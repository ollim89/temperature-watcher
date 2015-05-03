using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Configuration.WebApiSection
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

        [ConfigurationProperty("useAuth")]
        public bool UseAuth
        {
            get
            {
                return (bool)this["useAuth"];
            }
            set
            {
                this["useAuth"] = value;
            }
        }

        [ConfigurationProperty("username")]
        public string Username
        {
            get
            {
                return (string)this["username"];
            }
            set
            {
                this["username"] = value;
            }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }
    }
}
