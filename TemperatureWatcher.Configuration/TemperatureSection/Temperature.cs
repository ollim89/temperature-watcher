using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.Configuration.TimeToLeaveSection;
using TemperatureWatcher.Configuration.General;
using TemperatureWatcher.Configuration.Interfaces;

namespace TemperatureWatcher.Configuration.TemperatureSection
{
    public class Temperature : ConfigurationElement, IExternalPathElementContainer
    {
        [ConfigurationProperty("http")]
        public ExternalPath HttpSource
        {
            get
            {
                return (ExternalPath)this["http"];
            }
            set
            {
                this["http"] = value;
            }
        }

        [ConfigurationProperty("file")]
        public ExternalPath FileSource
        {
            get
            {
                return (ExternalPath)this["file"];
            }
            set
            {
                this["file"] = value;
            }
        }
    }
}
