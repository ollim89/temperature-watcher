using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TemperatureWatcher.Common;
using TemperatureWatcher.ConfigurationSection.ExecutionSection;

namespace TemperatureWatcher.Execution
{
    class ExecutableHandler
    {
        private TemperatureWatcher.ConfigurationSection.ExecutionSection.Execution _settings;
        private Timer _executableEndTimer;

        public bool IsExecuting { get; set; }

        public ExecutableHandler(TemperatureWatcher.ConfigurationSection.ExecutionSection.Execution settings, ElapsedEventHandler executableEndTimerHandler)
        {
            _settings = settings;
            IsExecuting = false;

            _executableEndTimer = new Timer();
            _executableEndTimer.AutoReset = false;
            _executableEndTimer.Elapsed += executableEndTimerHandler;
        }

        public void TurnOnExecutable(DateTime onUntil)
        {
            RunExecutable(true, onUntil);
        }

        public void TurnOnExecutable()
        {
            RunExecutable(true);
        }

        public void TurnOffExecutable()
        {
            RunExecutable(false);
        }

        private void RunExecutable(bool turnOn, DateTime? onUntil = null)
        {
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
                        onUntil.Value.AddHours(_settings.DelayShutdown.Hours);
                        onUntil.Value.AddMinutes(_settings.DelayShutdown.Minutes);
                        onUntil.Value.AddSeconds(_settings.DelayShutdown.Seconds);

                        _executableEndTimer.Interval = (onUntil.Value - DateTime.Now).Milliseconds;

                        _executableEndTimer.Start();
                    }

                    IsExecuting = true;
                }
                else
                {
                    IsExecuting = false;
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
