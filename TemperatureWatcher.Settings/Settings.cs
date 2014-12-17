using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Settings
{
    public class Settings
    {
        public SettingsSection settingsSection { get; set; }
        
        public Settings()
        {
            settingsSection = (SettingsSection)ConfigurationManager.GetSection("temperatureWatcherSettings");
        }
    }
}
