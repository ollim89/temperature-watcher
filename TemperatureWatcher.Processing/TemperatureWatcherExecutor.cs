using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using TemperatureWatcher.Common;
using TemperatureWatcher.Common.WebApiEventTypes;
using TemperatureWatcher.Common.WebApiResponseTypes;
using TemperatureWatcher.ConfigurationSection;
using TemperatureWatcher.ConfigurationSection.TimeToLeaveSection;
using TemperatureWatcher.Execution;
using TemperatureWatcher.Execution.Workers;
using TemperatureWatcher.Execution.Workers.Factories;

namespace TemperatureWatcher.Execution
{
    public class TemperatureWatcherExecutor
    {
        #region Fields
        private ExternalPathWorker _currentTemperatureWorker;
        private ExternalPathWorker _timeToLeaveWorker;
        private TimeToLeaveHandler _timeToLeaveHandler;
        private ExecutableHandler _executableHandler;
        private Timer _executionTimer;
        #endregion

        #region Properties
        public static TemperatureWatcherSettings Settings;
        #endregion

        #region Constructors/Destructors
        public TemperatureWatcherExecutor(TemperatureWatcherSettings settings)
        {
            //Get settings
            Settings = settings;

            //Set worker to get current temperature according to settings
            _currentTemperatureWorker = ExternalPathWorkerFactory.CreateExternalPathWorker(Settings.Temperature);
            _currentTemperatureWorker.StartWorker();

            //Set time to leave handling according to configuration
            if (Settings.TimeToLeave.WebServiceSource != null)
            {
                _timeToLeaveHandler = new TimeToLeaveHandler();
            }
            else
            {
                _timeToLeaveWorker = ExternalPathWorkerFactory.CreateExternalPathWorker(Settings.TimeToLeave);
                _timeToLeaveWorker.StartWorker();
                _timeToLeaveHandler = new TimeToLeaveHandler(_timeToLeaveWorker);
            }

            //Set execution timer
            _executionTimer = new Timer(333);
            _executionTimer.Elapsed += CheckToExecute;
            _executionTimer.AutoReset = false;
            _executionTimer.Start();

            _executableHandler = new ExecutableHandler(Settings.Execution);
        }
        #endregion

        #region WebApi
        public IWebApiResponse ReceiveWebApiCall(IWebApiEvent request)
        {
            if(typeof(GetExecutingStateEvent) == request)
            {
                return new GetExecutingStateResponse(_executableHandler.IsExecuting);
            }
            else if(typeof(SetLeaveTimeEvent) == request)
            {
                SetLeaveTimeEvent leaveTimeEvent = (SetLeaveTimeEvent)request;
                _timeToLeaveHandler.IsActive = leaveTimeEvent.IsActive;
                _timeToLeaveHandler.Hour = leaveTimeEvent.Hour;
                _timeToLeaveHandler.Minute = leaveTimeEvent.Minute;
            }
            else if(typeof(StartExecutableEvent) == request)
            {
                _executableHandler.RunExecutable(true);
            }
            else
            {
                throw new ArgumentException("Invalid web api event");
            }

            //Return null as default since all events does not return return data
            return null;
        }
        #endregion

        void CheckToExecute(object sender, ElapsedEventArgs e)
        {
            if(_executableHandler.IsExecuting)
            {
                
            }
            else if(!_executableHandler.IsExecuting && _timeToLeaveHandler.IsActive)
            {
                float currentTemperature;
                float.TryParse(_currentTemperatureWorker.Content, out currentTemperature);
                TimeSpan timeLeftToStart = _timeToLeaveHandler.GetTimeLeftToStartLevel(currentTemperature, Settings.StartLevels);

                //If the time left to start is less than a second, make sure to start not to miss it
                if(timeLeftToStart.Milliseconds < 1000)
                {
                    _executableHandler.RunExecutable(true);
                }
            }

            _executionTimer.Start();
        }

        public void StopExecutor()
        {
            _currentTemperatureWorker.StopWorker();
            _timeToLeaveWorker.StopWorker();
        }
    }
}
