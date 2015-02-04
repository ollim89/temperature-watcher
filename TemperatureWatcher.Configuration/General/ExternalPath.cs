using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.Configuration.TimeToLeaveSection;

namespace TemperatureWatcher.Configuration.General
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
                this["path"] = value;
            }
        }

        [ConfigurationProperty("contentMask")]
        public string ContentMask
        {
            get
            {
                return (string)this["contentMask"];
            }
            set
            {
                this["contentMask"] = value;
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
