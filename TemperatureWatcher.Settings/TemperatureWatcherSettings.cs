using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using TemperatureWatcher.ConfigurationSection.TimeToLeaveSection;
using TemperatureWatcher.ConfigurationSection.TemperatureSection;
using TemperatureWatcher.ConfigurationSection.StartLevelsSection;
using TemperatureWatcher.ConfigurationSection.ExecutionSection;

namespace TemperatureWatcher.ConfigurationSection
{
    public class TemperatureWatcherSettings : ConfigurationElement
    {
        [ConfigurationProperty("timeToLeaveSource")]
        public TimeToLeave TimeToLeave
        {
            get
            {
                return (TimeToLeave)this["timeToLeaveSource"];
            }
            set
            {
                this["timeToLeaveSource"] = value;
            }
        }

        [ConfigurationProperty("temperatureSource")]
        public Temperature Temperature
        {
            get
            {
                return (Temperature)this["temperatureSource"];
            }
            set
            {
                this["temperatureSource"] = value;
            }
        }

        [ConfigurationProperty("startLevels")]
        public StartLevel[] StartLevels
        {
            get
            {
                return (StartLevel[])this["startLevels"];
            }
            set
            {
                this["startLevels"] = value;
            }
        }

        [ConfigurationProperty("execution")]
        public Execution Execution
        {
            get
            {
                return (Execution)this["execution"];
            }
            set
            {
                this["execution"] = value;
            }
        }
    }
}
