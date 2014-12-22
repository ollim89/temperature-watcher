using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.ConfigurationSection.General;
using TemperatureWatcher.ConfigurationSection.TimeToLeaveSection;

namespace TemperatureWatcher.ConfigurationSection
{
    public class TimeToLeaveSourceFactory
    {
        public static ITimeToLeave CreateTimeToLeaveSetting()
        {
            TemperatureWatcherSettings settingsSection = (TemperatureWatcherSettings)ConfigurationManager.GetSection("temperatureWatcherSettings");

            if(settingsSection.TimeToLeave.HttpSource != null && settingsSection.TimeToLeave.HttpSource.Enabled)
            {
                return settingsSection.TimeToLeave.HttpSource;
            }
            else if (settingsSection.TimeToLeave.FileSource != null && settingsSection.TimeToLeave.FileSource.Enabled)
            {
                return settingsSection.TimeToLeave.FileSource;
            }
            else if(settingsSection.TimeToLeave.WebServiceSource != null && settingsSection.TimeToLeave.WebServiceSource.Enabled)
            {
                return settingsSection.TimeToLeave.WebServiceSource;
            }
            else
            {
                throw new Exception("No timeToLeave setting enabled/existing");
            }
        }
    }
}
