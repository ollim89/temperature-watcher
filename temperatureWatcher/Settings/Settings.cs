using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace temperatureWatcher
{
    public enum METHOD
    {
        HTTP, FILE
    }

    public struct StartLevel
    {
        public float Temperature { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }

    public class Settings
    {
        /* Reading the starttime */
        public METHOD TimeSettingsMethod { get; set; }
        public string TimeSettingPath { get; set; }
        
        public int TimeSettingReadIntervalHours { get; set; }
        public int TimeSettingReadIntervalMinutes { get; set; }
        public int TimeSettingReadIntervalSeconds { get; set; }

        /* Reading the current temperature */
        public METHOD TemperatureGetMethod { get; set; }
        public string TemperatureGetPath { get; set; }

        public int TemperatureReadIntervalHours { get; set; }
        public int TemperatureReadIntervalMinutes { get; set; }
        public int TemperatureReadIntervalSeconds { get; set; }

        /* List of temperaturelevels and their starttime */
        public List<StartLevel> StartLevels { get; set; }

        /* Executable */
        public string Executable { get; set; }
        public string FlagsOn { get; set; }
        public string FlagsOff { get; set; }

        public int DelayShutdownHours { get; set; }
        public int DelayShutdownMinutes { get; set; }
        public int DelayShutdownSeconds { get; set; }

        /* Webservice */
        public int WebServicePort { get; set; }

        public Settings(
            METHOD timeSettingsMethod,
            string timeSettingPath, 
            int timeSettingReadIntervalHours,
            int timeSettingReadIntervalMinutes,
            int timeSettingReadIntervalSeconds, 
            METHOD temperatureGetMethod,
            string temperatureGetPath,
            int temperatureReadIntervalHours,
            int temperatureReadIntervalMinutes,
            int temperatureReadIntervalSeconds,
            List<StartLevel> startLevels,
            string executable,
            string flagsOn,
            string flagsOff,
            int delayShutdownHours,
            int delayShutdownMinutes,
            int delayShutdownSeconds,
            int webServicePort
            )
        {
            TimeSettingsMethod = timeSettingsMethod;
            TimeSettingPath = timeSettingPath;
            TimeSettingReadIntervalHours = timeSettingReadIntervalHours;
            TimeSettingReadIntervalMinutes = timeSettingReadIntervalMinutes;
            TimeSettingReadIntervalSeconds = timeSettingReadIntervalSeconds;
            TemperatureGetMethod = temperatureGetMethod;
            TemperatureGetPath = temperatureGetPath;
            TemperatureReadIntervalHours = temperatureReadIntervalHours;
            TemperatureReadIntervalMinutes = temperatureReadIntervalMinutes;
            TemperatureReadIntervalSeconds = temperatureReadIntervalSeconds;
            StartLevels = startLevels;
            Executable = executable;
            FlagsOn = flagsOn;
            FlagsOff = flagsOff;
            DelayShutdownHours = delayShutdownHours;
            DelayShutdownMinutes = delayShutdownMinutes;
            DelayShutdownSeconds = delayShutdownSeconds;
            WebServicePort = webServicePort;
        }

        public Settings(string xmlFilePath)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(settings));
                settings xmlSettings = ser.Deserialize(new FileStream(xmlFilePath, FileMode.Open)) as settings;

                if (xmlSettings != null)
                {
                    this.Executable = xmlSettings.execution.executable;
                    this.FlagsOff = xmlSettings.execution.flags.off;
                    this.FlagsOn = xmlSettings.execution.flags.on;

                    this.DelayShutdownHours = xmlSettings.execution.delayShutdown.hours;
                    this.DelayShutdownMinutes = xmlSettings.execution.delayShutdown.minutes;
                    this.DelayShutdownSeconds = xmlSettings.execution.delayShutdown.seconds;

                    this.StartLevels = new List<StartLevel>();

                    foreach (settingsLevel l in xmlSettings.startlevels)
                    {
                        this.StartLevels.Add(
                            new StartLevel
                            {
                                Temperature = l.temperature,
                                Hours = l.hours,
                                Minutes = l.minutes,
                                Seconds = l.seconds
                            });
                    }

                    if (xmlSettings.temperature.method == "http")
                    {
                        this.TemperatureGetMethod = METHOD.HTTP;
                    }
                    else
                    {
                        this.TemperatureGetMethod = METHOD.FILE;
                    }

                    this.TemperatureGetPath = xmlSettings.temperature.path;
                    this.TemperatureReadIntervalHours = xmlSettings.temperature.readinterval.hours;
                    this.TemperatureReadIntervalMinutes = xmlSettings.temperature.readinterval.minutes;
                    this.TemperatureReadIntervalSeconds = xmlSettings.temperature.readinterval.seconds;
                    
                    if (xmlSettings.timefile.method == "http")
                    {
                        this.TimeSettingsMethod = METHOD.HTTP;
                    }
                    else
                    {
                        this.TimeSettingsMethod = METHOD.FILE;
                    }

                    this.TimeSettingPath = xmlSettings.timefile.path;
                    this.TimeSettingReadIntervalHours = xmlSettings.timefile.readinterval.hours;
                    this.TimeSettingReadIntervalMinutes = xmlSettings.timefile.readinterval.minutes;
                    this.TimeSettingReadIntervalSeconds = xmlSettings.timefile.readinterval.seconds; 
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
