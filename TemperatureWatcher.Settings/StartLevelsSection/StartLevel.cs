using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.ConfigurationSection.StartLevelsSection
{
    public class StartLevel : ConfigurationElement
    {
        [ConfigurationProperty("temperature")]
        public float Temperature
        {
            get
            {
                return (float)this["temperature"];
            }
            set
            {
                this["temperature"] = value;
            }
        }

        [ConfigurationProperty("hours")]
        public int Hours
        {
            get
            {
                return (int)this["hours"];
            }
            set
            {
                this["hours"] = value;
            }
        }

        [ConfigurationProperty("minutes")]
        public int Minutes
        {
            get
            {
                return (int)this["minutes"];
            }
            set
            {
                this["minutes"] = value;
            }
        }

        [ConfigurationProperty("seconds")]
        public int Seconds
        {
            get
            {
                return (int)this["seconds"];
            }
            set
            {
                this["seconds"] = value;
            }
        }
    }
}
