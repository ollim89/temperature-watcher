using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.Common;
using TemperatureWatcher.ConfigurationSection.ExecutionSection;

namespace TemperatureWatcher.Execution
{
    class ExecutableHandler
    {
        private TemperatureWatcher.ConfigurationSection.ExecutionSection.Execution _settings;

        public bool IsExecuting { get; set; }

        public ExecutableHandler(TemperatureWatcher.ConfigurationSection.ExecutionSection.Execution settings)
        {
            _settings = settings;
            IsExecuting = false;
        }

        public void RunExecutable(bool onFlags)
        {
            string flags;
            if (onFlags)
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

                if(onFlags)
                {
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
