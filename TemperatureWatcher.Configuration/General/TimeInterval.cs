using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Configuration.General
{
    public class TimeInterval : ConfigurationElement
    {
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
