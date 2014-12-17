using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Settings.TimeToLeaveSection
{
    class TimeToLeave : ConfigurationSection
    {
        [ConfigurationProperty("file")]
        public ExternalPath FileSource { get; set; }

        [ConfigurationProperty("http")]
        public ExternalPath HttpSource { get; set; }

        [ConfigurationProperty("webService")]
        public WebService WebServiceSource { get; set; }
    }
}
