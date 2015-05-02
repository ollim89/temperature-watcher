using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemperatureWatcher.Configuration.General;
using TemperatureWatcher.Execution;
using System.ServiceProcess;
using TemperatureWatcher.WebApi;
using TemperatureWatcher.Configuration;
using TemperatureWatcher.Common;
using System.Configuration;
using System.Diagnostics;

namespace TemperatureWatcher.Service
{
    class TemperatureWatcherService : ServiceBase
    {
        Config _settings; 
        Executor _executor;
        Initializer _webApi;

        protected override void OnStart(string[] args)
        {
            try
            {
                Trace.WriteLine("[TemperatureWatcher][Service][TemperatureWatcherService][OnStart] Getting settings");
                _settings = (Config)ConfigurationManager.GetSection("temperatureWatcherSettings");

                Trace.WriteLine("[TemperatureWatcher][Service][TemperatureWatcherService][OnStart] Initializing the service executor");
                _executor = new Executor(_settings);

                Trace.WriteLine("[TemperatureWatcher][Service][TemperatureWatcherService][OnStart] Initializing WebApi");
                _webApi = new Initializer(_settings, _executor.ReceiveWebApiCall);
            }
            catch(Exception e)
            {
                Trace.Write("[TemperatureWatcher][Service][TemperatureWatcherService][OnStart] The service failed to start with the following error: " + e.ToString() + Environment.NewLine);
                Logger.WriteEntry("[TemperatureWatcher][Service][TemperatureWatcherService][OnStart] The service failed to start with the following error: " + e.ToString(), EventLogEntryType.Error);
                throw e;
            }
        }

        protected override void OnStop()
        {
            try
            {
                _executor.StopExecutor();
            }
            catch(Exception e)
            {
                Trace.Write("[TemperatureWatcher][Service][TemperatureWatcherService][OnStop] The service failed to stop safely with the following error: " + e.ToString() + Environment.NewLine);
                Logger.WriteEntry("[TemperatureWatcher][Service][TemperatureWatcherService][OnStop] The service failed to stop safely with the following error: " + e.ToString(), EventLogEntryType.Error);
            }
        }

        private void InitializeComponent()
        {
            // 
            // TemperatureWatcherService
            // 
            this.ServiceName = "TemperatureWatcherService";

        }

        #region Depracted
        //public Settings settings { get; set; }
        //public DateTime nextLeaveTime;
        //private float currentTemperature;

        //public static string EventLogSource = "TemperatureWatcher";
        //public static string EventLogName = "Application";

        //private bool IsOn;
        //private bool IsActive = true;

        //public bool IsInstantStart { get; set; }
        //public  DateTime InstantStartStopTime { get; set; }

        //private DateTime StartTime;
        //private DateTime OffTime;

        //private string ExePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\";
        //private string XMLSettingsFile = "settings.xml";

        //private ServiceHost Host = null;

        //private TemperatureWatcherWebService WebService;

        //public TemperatureWatcherService()
        //{
        //    InitializeComponent();

        //    //Create source in eventlog if not exists
        //    if (!EventLog.SourceExists(EventLogSource))
        //    {
        //        EventLog.CreateEventSource(EventLogSource, EventLogName);
        //    }
        //}

        ///// <summary>
        ///// Method called when service is started, loads settingsfile and starts timers
        ///// </summary>
        ///// <param name="args"></param>
        //protected override void OnStart(string[] args)
        //{

        //    try
        //    {
        //        bool fileExists = false;

        //        try
        //        {
        //            if (File.Exists(ExePath + XMLSettingsFile))
        //            {
        //                fileExists = true;
        //            }
        //        }
        //        catch (IOException)
        //        {
        //            throw;
        //        }

        //        if (fileExists)
        //        {
        //            try
        //            {
        //                settings = new Settings(ExePath + XMLSettingsFile);
        //            }
        //            catch (Exception e)
        //            {
        //                throw new InvalidDataException(e.ToString());
        //            }

        //            //Write log to eventlog
        //            EventLog.WriteEntry(EventLogSource, "Reading leavetime from: " + settings.TimeSettingPath +
        //            " and temperature from: " + settings.TemperatureGetPath, EventLogEntryType.Information);

        //            TimePollTimer_Elapsed(new object(), new EventArgs());
        //            TemperaturePollTimer_Elapsed(new object(), new EventArgs());

        //            TimePollTimer.Enabled = true;
        //            TimePollTimer.Interval = (settings.TimeSettingReadIntervalHours * 1000 * 60 * 60) + (settings.TimeSettingReadIntervalMinutes * 1000 * 60) + (settings.TimeSettingReadIntervalSeconds * 1000);
        //            TimePollTimer.Start();

        //            TemperaturePollTimer.Enabled = true;
        //            TemperaturePollTimer.Interval = (settings.TemperatureReadIntervalHours * 1000 * 60 * 60) + (settings.TemperatureReadIntervalMinutes * 1000 * 60) + (settings.TemperatureReadIntervalSeconds * 1000);
        //            TemperaturePollTimer.Start();

        //            IsOn = false;

        //            ExecutionTimer.Enabled = true;
        //            ExecutionTimer.Start();
        //        }
        //        else
        //        {
        //            //Write log to eventlog
        //            EventLog.WriteEntry(EventLogSource, "XML-settings file cannot be found in install directory", EventLogEntryType.Error);
        //        }

        //        WebService = TemperatureWatcherWebService.Instance;
        //        WebService.TemperatureWatcher = this;
        //        RunWebService();
        //    }
        //    catch (InvalidDataException e)
        //    {
        //        EventLog.WriteEntry(EventLogSource, "File was found but settings could not be loaded. Errormessage: " + e.ToString(), EventLogEntryType.Error);
        //        this.Stop();
        //        this.Dispose();
        //    }
        //    catch (IOException e)
        //    {
        //        //Write log to eventlog
        //        EventLog.WriteEntry(EventLogSource, e.ToString(), EventLogEntryType.Error);
        //    }
        //    catch (Exception e)
        //    {
        //        //Write log to eventlog
        //        EventLog.WriteEntry(EventLogSource, "Error occured during initialization of service, errormessage: " + e.ToString(), EventLogEntryType.Error);
        //    }
        //}

        ///// <summary>
        ///// Method called when service is stopped, sends stop command to executable
        ///// </summary>
        //protected override void OnStop()
        //{
        //    if (Host != null)
        //    {
        //        StopWebService();
        //    }

        //    if (settings != null)
        //    {
        //        StopExecutable();
        //    }
        //}

        ///// <summary>
        ///// Method called when the timer for the temperaturepoll elapses. Gets the current temperature each time
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void TemperaturePollTimer_Elapsed(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (settings.TemperatureGetMethod == METHOD.HTTP)
        //        {
        //            currentTemperature = GetCurrentTemperatureHTTP();
        //        }
        //        else if (settings.TemperatureGetMethod == METHOD.FILE)
        //        {
        //            currentTemperature = GetCurrentTemperatureFILE();
        //        }
        //        else
        //        {
        //            throw new Exception("No method was specified in the XML settings file for how to get current temperature");
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        //Write log to eventlog
        //        EventLog.WriteEntry(EventLogSource, exc.ToString(), EventLogEntryType.Error);
        //    }
        //}

        ///// <summary>
        ///// Method gets called when the timer for the timepoll timer has elapsed. Gets the currently set leave time, and calculates when it is going to happen next time.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void TimePollTimer_Elapsed(object sender, EventArgs e)
        //{
        //    DateTime nextLeaveTimeCurrent = nextLeaveTime;
        //    try
        //    {
        //        if (settings.TimeSettingsMethod == METHOD.FILE)
        //        {
        //            nextLeaveTime = GetTimeFromFILE();
        //        }
        //        else
        //        {
        //            nextLeaveTime = GetTimeByHTTP();
        //        }

        //        //Check if the time has passed and set next leave time to tomorrow
        //        if (DateTime.Now > nextLeaveTime)
        //        {
        //            nextLeaveTime = nextLeaveTime.AddDays(1);
        //        }

        //        if (nextLeaveTime != nextLeaveTimeCurrent)
        //        {
        //            //Write log to eventlog
        //            EventLog.WriteEntry(EventLogSource, "Leavetime is set to: " + nextLeaveTime.ToString(), EventLogEntryType.Information);
        //        }
        //    }
        //    catch (IOException exc)
        //    {
        //        //Write log to eventlog
        //        EventLog.WriteEntry(EventLogSource, exc.ToString(), EventLogEntryType.Error);
        //    }
        //}

        //private void ExecutionTimer_Elapsed(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //If schedualer is active
        //        if (IsActive)
        //        {
        //            //If is instantly started allready
        //            if (IsInstantStart)
        //            {
        //                if (DateTime.Now > InstantStartStopTime)
        //                {
        //                    StopExecutable();
        //                }
        //            }
        //            //If not instantly started allready
        //            else
        //            {
        //                if (nextLeaveTime != null)
        //                {
        //                    StartTime = nextLeaveTime;

        //                    settings.StartLevels.OrderByDescending(x => x.Temperature);

        //                    //If current temperature is less than the temperature in first startlevel
        //                    if (currentTemperature <= settings.StartLevels.First().Temperature)
        //                    {
        //                        //Loop through startlevels
        //                        for (int i = 0; i < settings.StartLevels.Count; i++)
        //                        {
        //                            //If last startlevel set starttime to specified time of startlevel, else compare if current temperature belongs to the startlevel in loop
        //                            if (((i + 1) == settings.StartLevels.Count) || (currentTemperature <= settings.StartLevels[i].Temperature && currentTemperature > settings.StartLevels[i + 1].Temperature))
        //                            {
        //                                StartTime = StartTime.AddHours(-settings.StartLevels[i].Hours).AddMinutes(-settings.StartLevels[i].Minutes).AddSeconds(-settings.StartLevels[i].Seconds);
        //                                break;
        //                            }
        //                        }
        //                    }

        //                    if (!IsOn || OffTime == null || (IsOn && OffTime != null && nextLeaveTime.AddHours(settings.DelayShutdownHours).AddMinutes(settings.DelayShutdownMinutes).AddSeconds(settings.DelayShutdownSeconds) <= OffTime))
        //                    {
        //                        OffTime = nextLeaveTime.AddHours(settings.DelayShutdownHours).AddMinutes(settings.DelayShutdownMinutes).AddSeconds(settings.DelayShutdownSeconds);
        //                    }

        //                    //If warmer is to be turned off
        //                    if (IsOn && DateTime.Now > OffTime)
        //                    {
        //                        StopExecutable();
        //                        TimePollTimer_Elapsed(new object(), new EventArgs());
        //                        TemperaturePollTimer_Elapsed(new object(), new EventArgs());
        //                    }
        //                    //If warmer is to be turned on
        //                    else if (!IsOn && DateTime.Now >= StartTime && DateTime.Now < OffTime)
        //                    {
        //                        StartExecutable(OffTime);
        //                        TimePollTimer_Elapsed(new object(), new EventArgs());
        //                        TemperaturePollTimer_Elapsed(new object(), new EventArgs());
        //                    }
        //                }
        //            }
        //        }
        //        //Stop executable if schedualer is inactive but executable is started without instantstart or if it is instantly started and this time has passed
        //        else if ((!IsActive && IsOn && !IsInstantStart) || (IsInstantStart && DateTime.Now > InstantStartStopTime))
        //        {
        //            StopExecutable();
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        //Write log to eventlog
        //        EventLog.WriteEntry(EventLogSource, exc.ToString(), EventLogEntryType.Error);
        //    }
        //}

        //public bool StartExecutable(DateTime offTime)
        //{
        //    IsOn = true;
        //    Process process = null;

        //    try
        //    {
        //        process = Process.Start(settings.Executable, settings.FlagsOn);
        //    }
        //    catch (Exception e)
        //    {
        //        EventLog.WriteEntry(EventLogSource, "Process could not be started, error: " + e.ToString(), EventLogEntryType.Error);
        //        throw;
        //    }

        //    if (process != null)
        //    {
        //        //Write log to eventlog
        //        EventLog.WriteEntry(EventLogSource, "Executable On-flags sent, warmer will be on until: " + offTime.ToString(), EventLogEntryType.Information);
        //        return true;
        //    }
        //    else
        //    {
        //        EventLog.WriteEntry(EventLogSource, "Process could not be started", EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        //public void StopExecutable()
        //{
        //    IsOn = false;
        //    IsInstantStart = false;
        //    Process.Start(settings.Executable, settings.FlagsOff);

        //    //Write log to eventlog
        //    EventLog.WriteEntry(EventLogSource, "Executable Off-flags sent", EventLogEntryType.Information);
        //}

        //private float GetCurrentTemperatureHTTP()
        //{
        //    try
        //    {
        //        WebClient client = new WebClient();
        //        string strTemperature = client.DownloadString(settings.TemperatureGetPath);
        //        strTemperature.Trim();

        //        Regex match = new Regex("[-,0-9]*[.][0-9]*");
        //        strTemperature = match.Match(strTemperature).ToString();

        //        float temperature;
        //        float.TryParse(strTemperature, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out temperature);

        //        return temperature;
        //    }
        //    catch (WebException)
        //    {
        //        throw;
        //    }
        //}

        //private float GetCurrentTemperatureFILE()
        //{
        //    try
        //    {
        //        StreamReader sr = new StreamReader(settings.TemperatureGetPath);

        //        string strTemperature = sr.ReadLine();
        //        sr.Close();

        //        float temperature;
        //        float.TryParse(strTemperature, out temperature);

        //        return temperature;
        //    }
        //    catch (IOException)
        //    {
        //        throw;
        //    }
        //}

        //private DateTime GetTimeByHTTP()
        //{
        //    try
        //    {
        //        WebClient client = new WebClient();

        //        string strTime = client.DownloadString(settings.TimeSettingPath);
        //        strTime.Trim();

        //        ChangeActiveState(strTime);

        //        Regex match = new Regex("[0-9]*[:][0-9]*");
        //        strTime = match.Match(strTime).ToString();

        //        int hour, minute;
        //        int.TryParse(strTime.Split(':')[0], out hour);
        //        int.TryParse(strTime.Split(':')[1], out minute);
        //        DateTime time = DateTime.Now.Date.AddHours(hour).AddMinutes(minute);

        //        return time;
        //    }
        //    catch (WebException)
        //    {
        //        throw;
        //    }
        //}

        //private DateTime GetTimeFromFILE()
        //{
        //    try
        //    {
        //        //Get the file where the leave-time is specified
        //        StreamReader sr = new StreamReader(settings.TimeSettingPath);

        //        //Explode to separate hour and minute
        //        string line = sr.ReadLine();
        //        sr.Close();

        //        ChangeActiveState(line);

        //        Regex match = new Regex("[0-9]*[:][0-9]*");
        //        string strTime = match.Match(line).ToString();

        //        int hour, minute;
        //        int.TryParse(strTime.Split(':')[0], out hour);
        //        int.TryParse(strTime.Split(':')[1], out minute);

        //        //Set leave-time-object to specified time today
        //        DateTime time = DateTime.Now.Date.AddHours(hour).AddMinutes(minute);

        //        return time;
        //    }
        //    catch (IOException)
        //    {
        //        throw;
        //    }
        //}

        //private void ChangeActiveState(string searchString)
        //{
        //    Regex match = new Regex("inactive");
        //    bool IsActiveBefore = IsActive;
        //    if (match.Match(searchString).Success)
        //    {
        //        IsActive = false;
        //    }
        //    else
        //    {
        //        IsActive = true;
        //    }

        //    if (IsActiveBefore != IsActive)
        //    {
        //        //Write log to eventlog
        //        EventLog.WriteEntry(EventLogSource, "Activestate changed, new activestate is: " + (IsActive ? "Active" : "Inactive"), EventLogEntryType.Information);
        //    }
        //}

        //private void RunWebService()
        //{
        //    this.Host = new ServiceHost(WebService);
        //    this.Host.Open();

        //    EventLog.WriteEntry(EventLogSource, "Webservice started", EventLogEntryType.Information);
        //}

        //private void StopWebService()
        //{
        //    if (this.Host != null)
        //    {
        //        try
        //        {
        //            this.Host.Close();
        //        }
        //        catch (Exception e)
        //        {
        //            EventLog.WriteEntry(EventLogSource, "Could not close host, errormessage: " + e.ToString(), EventLogEntryType.Error);
        //        }

        //        EventLog.WriteEntry(EventLogSource, "Webservice closed", EventLogEntryType.Information);
        //    }
        //}
        #endregion
    }
}
