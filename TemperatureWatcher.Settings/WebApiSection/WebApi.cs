using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.ConfigurationSection.WebApiSection
{
    public class WebApi : ConfigurationElement
    {
        [ConfigurationProperty("url")]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
            set
            {
                this["string"] = value;
            }
        }
    }
}
