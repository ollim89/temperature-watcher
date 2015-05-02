using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using TemperatureWatcher.Common;
using TemperatureWatcher.Common.WebApiEventTypes;
using TemperatureWatcher.Common.WebApiResponseTypes;
using TemperatureWatcher.Configuration;
using TemperatureWatcher.Configuration.TimeToLeaveSection;
using TemperatureWatcher.Execution;
using TemperatureWatcher.Execution.Workers;
using TemperatureWatcher.Execution.Workers.Factories;

namespace TemperatureWatcher.Execution
{
    public class Executor
    {
        #region Fields
        private ExternalPathWorker<float> _currentTemperatureWorker;
        private ExternalPathWorker<string> _scheduleWorker;
        private ScheduleHandler _scheduleHandler;
        private ExecutableHandler _executableHandler;
        private object _locker = new object();
        #endregion

        #region Properties
        public static Config Settings;
        #endregion

        #region Constructors/Destructors
        public Executor(Config settings)
        {
            //Get settings
            Settings = settings;

            //Create executable handler
            Trace.WriteLine("[TemperatureWatcher][Execution][Executor][Constructor] Initializing executable handler");
            _executableHandler = new ExecutableHandler(Settings.Execution, OnExecutableEndTime);

            //Affects the schedule handler
            bool onlyUseWebApiToUpdateSchedule = true;

            //Create schedule worker if polling type of source
            if ((Settings.TimeToLeave.HttpSource != null && Settings.TimeToLeave.HttpSource.Enabled) || 
                (Settings.TimeToLeave.FileSource != null && Settings.TimeToLeave.FileSource.Enabled))
            {
                Trace.WriteLine("[TemperatureWatcher][Execution][Executor][Constructor] Getting time to leave by polling external source, initializing worker to poll external source");
                _scheduleWorker = ExternalPathWorkerFactory.CreateExternalPathWorker<string>(Settings.TimeToLeave, OnScheduleUpdate);
                onlyUseWebApiToUpdateSchedule = false;
            }

            //Create schedule handler
            Trace.WriteLine("[TemperatureWatcher][Execution][Executor][Constructor] Initializing schedule handler");
            _scheduleHandler = new ScheduleHandler(settings.StartLevels, OnScheduledExecutionTime, onlyUseWebApiToUpdateSchedule);

            //Set worker to get current temperature according to settings
            Trace.WriteLine("[TemperatureWatcher][Execution][Executor][Constructor] Initializing worker to poll external source for current temperature");
            _currentTemperatureWorker = ExternalPathWorkerFactory.CreateExternalPathWorker<float>(Settings.Temperature, OnCurrentTemperatureUpdate);

            //Start workers
            Trace.WriteLine("[TemperatureWatcher][Execution][Executor][Constructor] Start worker to get current temperature from external source");
            _currentTemperatureWorker.StartWorker();
            if(_scheduleWorker != null)
            {
                _scheduleWorker.StartWorker();
            }
        }
        #endregion

        #region WebApi
        /// <summary>
        /// Callback method for WebApi calls
        /// </summary>
        /// <param name="request">The request sent to the webapi</param>
        /// <returns>Returns different responses depending on request, or null if no response is required by the specific operation</returns>
        public IWebApiResponse ReceiveWebApiCall(IWebApiEvent request)
        {
            Trace.WriteLine("[TemperatureWatcher][Execution][Executor][ReceiveWebApiCall] Received WebApi call of type: " + request.GetType().Name);
            
            //Gets the executing status
            if(typeof(GetExecutingStateEvent) == request.GetType())
            {
                return new GetExecutingStateResponse(
                    _executableHandler.IsExecuting, 
                    _scheduleHandler.Hour,
                    _scheduleHandler.Minute,
                    _scheduleHandler.IsActive);
            }
            //Update schedule
            else if(typeof(SetScheduleEvent) == request.GetType())
            {
                SetScheduleEvent scheduleEvent = (SetScheduleEvent)request;
                if (scheduleEvent.IsActive)
                {
                    _scheduleHandler.UpdateSchedule(scheduleEvent.Hour, scheduleEvent.Minute, DateTime.Now);
                }
                else
                {
                    _scheduleHandler.Inactivate();
                }

                lock (_locker)
                {
                    //Update the file source if using file
                    if (Settings.TimeToLeave.FileSource.Enabled)
                    {
                        using (StreamWriter sw = new StreamWriter(Executor.Settings.TimeToLeave.FileSource.Path, false))
                        {
                            sw.WriteLine(string.Format("{0}:{1};{2}", scheduleEvent.Hour, scheduleEvent.Minute, scheduleEvent.IsActive.ToString().ToLower()));
                        }
                    }
                }
            }
            //Instant run of executable
            else if(typeof(ControlExecutableEvent) == request.GetType())
            {
                ControlExecutableEvent controlExecutableEvent = (ControlExecutableEvent)request;

                if (controlExecutableEvent.SendOnFlags)
                {
                    if (controlExecutableEvent.MinutesToKeepRunning > 0)
                    {
                        _executableHandler.TurnOnExecutable(
                            DateTime.Now.AddMinutes(controlExecutableEvent.MinutesToKeepRunning));
                    }
                    else
                    {
                        _executableHandler.TurnOnExecutable();
                    }
                }
                else
                {
                    _executableHandler.TurnOffExecutable();
                    _scheduleHandler.ResetTimer(true);
                }
            }
            //Get the current temperature
            else if(typeof(GetCurrentTemperatureEvent) == request.GetType())
            {
                return new GetCurrentTemperatureResponse(_scheduleHandler.CurrentTemperature, _scheduleHandler.CurrentTemperatureUpdated);
            }
            //Throw exception if no recognized event
            else
            {
                throw new ArgumentException("Invalid web api event");
            }

            //Return null as default since all events does not return return data
            return null;
        }
        #endregion

        #region On worker updated
        public void OnCurrentTemperatureUpdate(float temperature, DateTime temperatureUpdated)
        {
            _scheduleHandler.UpdateCurrentTemperature(temperature, temperatureUpdated);
        }

        public void OnScheduleUpdate(string schedule, DateTime scheduleUpdated)
        {
            string[] scheduleParts = schedule.Split(';');
            
            int hour, minute;
            int.TryParse(scheduleParts[0].Split(':')[0], out hour);
            int.TryParse(scheduleParts[0].Split(':')[1], out minute);

            bool isActive;
            bool.TryParse(scheduleParts[1], out isActive);
            
            if (isActive)
            {
                _scheduleHandler.UpdateSchedule(hour, minute, scheduleUpdated);
            }
            else
            {
                _scheduleHandler.Inactivate();
            }
        }
        #endregion

        #region Execution events
        public void OnScheduledExecutionTime(object sender, ElapsedEventArgs e)
        {
            if (!_executableHandler.IsExecuting)
            {
                _executableHandler.TurnOnExecutable(_scheduleHandler.GetNextScheduledTime(null, false));
            }
        }

        public void OnExecutableEndTime(object sender, ElapsedEventArgs e)
        {
            _executableHandler.TurnOffExecutable();
            _scheduleHandler.ResetTimer();
        }
        #endregion

        public void StopExecutor()
        {
            _currentTemperatureWorker.StopWorker();

            if (_scheduleWorker != null)
            {
                _scheduleWorker.StopWorker();
            }
        }
    }
}
