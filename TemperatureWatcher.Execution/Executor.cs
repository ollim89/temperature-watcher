using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        #endregion

        #region Properties
        public static Config Settings;
        #endregion

        #region Constructors/Destructors
        public Executor(Config settings)
        {
            //Get settings
            Settings = settings;

            _executableHandler = new ExecutableHandler(Settings.Execution, OnExecutableEndTime);
            _scheduleHandler = new ScheduleHandler(settings.StartLevels, OnScheduledExecutionTime);
            
            if (Settings.TimeToLeave.HttpSource != null || Settings.TimeToLeave.FileSource != null)
            {
                _scheduleWorker = ExternalPathWorkerFactory.CreateExternalPathWorker<string>(Settings.TimeToLeave, OnScheduleUpdate);
                _scheduleWorker.StartWorker();
            }

            //Set worker to get current temperature according to settings
            _currentTemperatureWorker = ExternalPathWorkerFactory.CreateExternalPathWorker<float>(Settings.Temperature, OnCurrentTemperatureUpdate);
            _currentTemperatureWorker.StartWorker();
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
            //Gets the executing status
            if(typeof(GetExecutingStateEvent) == request.GetType())
            {
                return new GetExecutingStateResponse(_executableHandler.IsExecuting);
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
            }
            //Instant run of executable
            else if(typeof(ControlExecutableEvent) == request.GetType())
            {
                ControlExecutableEvent controlExecutableEvent = (ControlExecutableEvent)request;

                if (controlExecutableEvent.SendOnFlags)
                {
                    _executableHandler.TurnOnExecutable();
                }
                else
                {
                    _executableHandler.TurnOffExecutable();
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
                _executableHandler.TurnOnExecutable(_scheduleHandler.GetNextScheduledTime());
            }
            else
            {
                _scheduleHandler.ResetTimer();
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
            _scheduleWorker.StopWorker();
        }
    }
}
