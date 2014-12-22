﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.ConfigurationSection.General;

namespace TemperatureWatcher.ConfigurationSection.TimeToLeaveSection
{
    public class TimeToLeave : ConfigurationSection
    {
        [ConfigurationProperty("file")]
        public ExternalPath FileSource
        {
            get
            {
                return (ExternalPath)this["file"];
            }
            set
            {
                this["file"] = value;
            }
        }

        [ConfigurationProperty("http")]
        public ExternalPath HttpSource
        {
            get
            {
                return (ExternalPath)this["http"];
            }
            set
            {
                this["http"] = value;
            }
        }

        [ConfigurationProperty("webService")]
        public WebService WebServiceSource
        {
            get
            {
                return (WebService)this["webService"];
            }
            set
            {
                this["webService"] = value;
            }
        }
    }
}
