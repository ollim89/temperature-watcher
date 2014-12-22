using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.ConfigurationSection;

namespace TemperatureWatcher.Service.Settings
{
    class Executor
    {
        #region Fields
        private DateTime _leaveTime;
        private float _currentTemperature;
        private bool _active;
        private TemperatureWatcherSettings _settings;
        #endregion

        #region Properties
        public DateTime LeaveTime 
        {
            get { return _leaveTime; }
            set { _leaveTime = value; }
        }
        public float CurrentTemperature
        {
            get { return _currentTemperature; }
            set { _currentTemperature = value; }
        }
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }
        #endregion

        public Executor()
        {
            _settings = new TemperatureWatcherSettings();
        }

        public void RunExecutable(bool onFlags)
        {
            string flags;
            if (onFlags)
            {
                flags = _settings.Execution.Flags.On;
            }
            else
            {
                flags = _settings.Execution.Flags.Off;
            }

            Process process = null;

            try
            {
                process = Process.Start(_settings.Execution.Executable, flags);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(TemperatureWatcherService.EventLogSource, "Process could not be started, error: " + e.ToString(), EventLogEntryType.Error);
                throw;
            }

            if (process != null)
            {
                //Write log to eventlog
                EventLog.WriteEntry(TemperatureWatcherService.EventLogSource, "Executable On-flags sent, warmer will be on until: " + offTime.ToString(), EventLogEntryType.Information);
            }
            else
            {
                EventLog.WriteEntry(TemperatureWatcherService.EventLogSource, "Process could not be started", EventLogEntryType.Error);
            }
        }
    }
}
