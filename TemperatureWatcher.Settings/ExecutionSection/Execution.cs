using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.ConfigurationSection.General;

namespace TemperatureWatcher.ConfigurationSection.ExecutionSection
{
    public class Execution : ConfigurationElement
    {
        [ConfigurationProperty("executable")]
        public string Executable
        {
            get
            {
                return (Flags)this["executable"];
            }
            set
            {
                this["executable"] = value;
            }
        }

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
