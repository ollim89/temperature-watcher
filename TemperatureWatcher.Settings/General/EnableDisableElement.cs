using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.ConfigurationSection.General
{
    public class EnableDisableElement : ConfigurationElement
    {
        [ConfigurationPropertyAttribute("enabled")]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
    }
}
