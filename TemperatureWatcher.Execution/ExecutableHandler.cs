using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TemperatureWatcher.Common;
using TemperatureWatcher.Configuration.ExecutionSection;

namespace TemperatureWatcher.Execution
{
    class ExecutableHandler
    {
        private TemperatureWatcher.Configuration.ExecutionSection.Execution _settings;
        private Timer _executableEndTimer;
        private readonly object _locker = new object();
        private bool _isExecuting;

        #region Properties
        public bool IsExecuting
        {
            get
            {
                lock(_locker)
                {
                    return _isExecuting;
                }
            }
        }
        #endregion

        public ExecutableHandler(TemperatureWatcher.Configuration.ExecutionSection.Execution settings, ElapsedEventHandler executableEndTimerHandler)
        {
            _settings = settings;
            _isExecuting = false;

            _executableEndTimer = new Timer();
            _executableEndTimer.AutoReset = false;
            _executableEndTimer.Elapsed += executableEndTimerHandler;
        }

        public void TurnOnExecutable(DateTime onUntil)
        {
            lock(_locker)
            {
                if(_isExecuting)
                {
                    return;
                }

                RunExecutable(true, onUntil);
            }
        }

        public void TurnOnExecutable()
        {
            lock (_locker)
            {
                if(_isExecuting)
                {
                    return;
                }

                RunExecutable(true);
            }
        }

        public void TurnOffExecutable()
        {
            lock (_locker)
            {
                if(!_isExecuting)
                {
                    return;
                }

                RunExecutable(false);
            }
        }

        private void RunExecutable(bool turnOn, DateTime? onUntil = null)
        {
            Trace.WriteLine("[TemperatureWatcher][Execution][ExecutableHandler][RunExecutable] Will run executable with " + (turnOn ? "on" : "off") + " flags");
            string flags;
            if (turnOn)
            {
                flags = _settings.Flags.On;
            }
            else
            {
                flags = _settings.Flags.Off;
            }

            Process process = null;

            try
            {
                process = Process.Start(_settings.Executable, flags);

                if(turnOn)
                {
                    if (onUntil.HasValue)
                    {
                        onUntil = onUntil.Value.AddHours(_settings.DelayShutdown.Hours);
                        onUntil = onUntil.Value.AddMinutes(_settings.DelayShutdown.Minutes);
                        onUntil = onUntil.Value.AddSeconds(_settings.DelayShutdown.Seconds);

                        _executableEndTimer.Interval = (onUntil.Value - DateTime.Now).TotalMilliseconds;

                        _executableEndTimer.Start();
                    }

                    _isExecuting = true;
                }
                else
                {
                    _isExecuting = false;
                }
            }
            catch (Exception e)
            {
                Logger.WriteEntry("Process could not be started, error: " + e.ToString(), EventLogEntryType.Error);
                throw;
            }
        }
    }
}
