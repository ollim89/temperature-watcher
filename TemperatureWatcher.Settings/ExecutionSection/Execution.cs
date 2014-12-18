using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.Settings.General;

namespace TemperatureWatcher.Settings.ExecutionSection
{
    public class Execution : ConfigurationElement
    {
        [ConfigurationProperty("flags")]
        public Flags Flags
        {
            get
            {
                return (Flags)this["flags"];
            }
            set
            {
                this["flags"] = value;
            }
        }

        [ConfigurationProperty("delayShutdownAfterTimePassed")]
        public TimeInterval DelayShutdown
        {
            get
            {
                return (TimeInterval)this["delayShutdown"];
            }
            set
            {
                this["delayShutdown"] = value;
            }
        }
    }
}
