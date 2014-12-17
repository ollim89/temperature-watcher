using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Settings.General
{
    public class ExternalPath : EnableDisableElement
    {
        [ConfigurationProperty("path")]
        public string Path 
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["value"] = value;
            }
        }

        [ConfigurationProperty("readInterval")]
        public TimeInterval ReadInterval { 
            get
            {
                return (TimeInterval)this["readInterval"];   
            }
            set
            {
                this["readInterval"] = value;
            }
        }
    }
}
