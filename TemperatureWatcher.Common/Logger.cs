using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Common
{
    public static class Logger
    {
        private static string _eventLogSource = "TemperatureWatcher";
        private static string _eventLogName = "Application";

        public static void WriteEntry(string message, EventLogEntryType type)
        {
            EventLog.WriteEntry(_eventLogSource, message, type);
        }
    }
}
