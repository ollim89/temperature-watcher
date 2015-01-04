using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Execution
{
    class ExecutableHandler
    {
        public static void RunExecutable(bool onFlags)
        {
            string flags;
            if (onFlags)
            {
                flags = TemperatureWatcherExecutor.Settings.Execution.Flags.On;
            }
            else
            {
                flags = TemperatureWatcherExecutor.Settings.Execution.Flags.Off;
            }

            Process process = null;

            try
            {
                process = Process.Start(TemperatureWatcherExecutor.Settings.Execution.Executable, flags);
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(TemperatureWatcherExecutor.EventLogSource, "Process could not be started, error: " + e.ToString(), EventLogEntryType.Error);
                throw;
            }

            if (process != null)
            {
                //Write log to eventlog
                EventLog.WriteEntry(TemperatureWatcherExecutor.EventLogSource, "Executable On-flags sent, warmer will be on until: " + offTime.ToString(), EventLogEntryType.Information);
            }
            else
            {
                EventLog.WriteEntry(TemperatureWatcherExecutor.EventLogSource, "Process could not be started", EventLogEntryType.Error);
            }
        }
    }
}
