using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.ConfigurationSection.TimeToLeaveSection;
using TemperatureWatcher.ConfigurationSection.General;

namespace TemperatureWatcher.ConfigurationSection.TemperatureSection
{
    public class Temperature : ConfigurationElement
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
