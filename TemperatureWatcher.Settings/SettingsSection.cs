using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using TemperatureWatcher.Settings.TimeToLeaveSection;
using TemperatureWatcher.Settings.TemperatureSection;
using TemperatureWatcher.Settings.StartLevelsSection;

namespace TemperatureWatcher.Settings
{
    public class SettingsSection : ConfigurationElement
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
    }
}
