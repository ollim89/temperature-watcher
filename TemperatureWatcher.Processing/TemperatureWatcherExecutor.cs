using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
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
        private Timer _executionTimer;
        #endregion

        #region Properties
        public static string EventLogSource = "TemperatureWatcher";
        public static string EventLogName = "Application";

        public static TemperatureWatcherSettings Settings;
        #endregion

        public TemperatureWatcherExecutor()
        {
            //Get settings
            Settings = new TemperatureWatcherSettings();

            //Set worker to get current temperature according to settings
            _currentTemperatureWorker = ExternalPathWorkerFactory.CreateExternalPathWorker(Settings.Temperature);
            _currentTemperatureWorker.StartWorker();

            //Set time to leave handling according to configuration
            if (Settings.TimeToLeave.WebServiceSource != null)
            {

            }
            else
            {
                _timeToLeaveWorker = ExternalPathWorkerFactory.CreateExternalPathWorker(Settings.TimeToLeave);
                _timeToLeaveWorker.StartWorker();
            }

            //Set execution timer
            _executionTimer = new Timer(100);
            _executionTimer.Elapsed += CheckToExecute;
            _executionTimer.Start();
        }

        void CheckToExecute(object sender, ElapsedEventArgs e)
        {

        }

        public void StopExecutor()
        {
            _currentTemperatureWorker.StopWorker();
            _timeToLeaveWorker.StopWorker();
        }
    }
}
