using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.ConfigurationSection.TimeToLeaveSection;

namespace TemperatureWatcher.ConfigurationSection.General
{
    public class ExternalPath : EnableDisableElement, ITimeToLeave
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
