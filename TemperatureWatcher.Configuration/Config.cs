using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using TemperatureWatcher.Configuration.TimeToLeaveSection;
using TemperatureWatcher.Configuration.TemperatureSection;
using TemperatureWatcher.Configuration.StartLevelsSection;
using TemperatureWatcher.Configuration.ExecutionSection;
using TemperatureWatcher.Configuration.WebApiSection;

namespace TemperatureWatcher.Configuration
{
    public class Config : ConfigurationSection
    {
        private static Config _instance;

        private Config() { }

        public static Config GetInstance()
        {
            if(_instance == null)
            {
                _instance = (Config)ConfigurationManager.GetSection("temperatureWatcherSettings");
            }

            return _instance;
        }

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

        [ConfigurationProperty("startLevels", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StartLevelCollection))]
        public StartLevelCollection StartLevels
        {
            get
            {
                return (StartLevelCollection)this["startLevels"];
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

        [ConfigurationProperty("webApi")]
        public WebApi WebApi
        {
            get
            {
                return (WebApi)this["webApi"];
            }
            set
            {
                this["webApi"] = value;
            }
        }
    }
}
