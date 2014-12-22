using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.ConfigurationSection.ExecutionSection
{
    public class Flags : ConfigurationElement
    {
        [ConfigurationProperty("on")]
        public string On 
        {
            get
            {
                return (string)this["on"];
            }
            set
            {
                this["on"] = value;
            }
        }

        [ConfigurationProperty("off")]
        public string Off
        {
            get
            {
                return (string)this["off"];
            }
            set
            {
                this["off"] = value;
            }
        }
    }
}
