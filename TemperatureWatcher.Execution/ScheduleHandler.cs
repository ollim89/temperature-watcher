using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TemperatureWatcher.Configuration.StartLevelsSection;
using TemperatureWatcher.Execution.Properties;
using TemperatureWatcher.Execution.Workers;

namespace TemperatureWatcher.Execution
{
    class ScheduleHandler
    {
        private static object _locker = new object();
        private StartLevel[] _startLevels;
        private int _hour;
        private bool _hourIsSet;
        private int _minute;
        private bool _minuteIsSet;
        private DateTime _scheduleUpdated;
        private Timer _timer;
        private float _currentTemperature;
        private DateTime _currentTemperatureUpdated;
        private bool _onlyUseWebApiToUpdateSchedule;
        private DateTime _lastExecutionEndedAt;

        #region Properties
        public float CurrentTemperature
        {
            get
            {
                lock (_locker)
                {
                    return _currentTemperature;
                }
            }
        }

        public DateTime CurrentTemperatureUpdated
        {
            get
            {
                lock (_locker)
                {
                    return _currentTemperatureUpdated;
                }
            }
        }

        public int Hour
        {
            get
            {
                lock (_locker)
                {
                    return _hour;
                }
            }
        }
        public int Minute
        {
            get
            {
                lock (_locker)
                {
                    return _minute;
                }
            }
        }
        public bool IsActive
        {
            get
            {
                lock (_locker)
                {
                    return _timer.Enabled;
                }
            }
        }
        #endregion

        #region Constructor(s)
        public ScheduleHandler(StartLevelCollection startLevelCollection, ElapsedEventHandler scheduleTimerElapsedHandler, bool onlyUseWebApiToUpdateSchedule)
        {
            //Initialize values
            _hourIsSet = false;
            _minuteIsSet = false;
            _currentTemperatureUpdated = DateTime.MinValue;
            _onlyUseWebApiToUpdateSchedule = onlyUseWebApiToUpdateSchedule;
            _lastExecutionEndedAt = DateTime.MinValue;

            if(_onlyUseWebApiToUpdateSchedule)
            {
                _hour = (int)Settings.Default["Hour"];
                _minute = (int)Settings.Default["Minute"];
                _hourIsSet = true;
                _minuteIsSet = true;

                if((bool)Settings.Default["IsActive"])
                {
                    ResetTimer();
                }
            }

            StartLevel[] startLevels = new StartLevel[startLevelCollection.Count];
            for(int i = 0; i < startLevelCollection.Count; i++)
            {
                startLevels[i] = startLevelCollection[i];
            }

            //Sort start levels by temperature so that the coldest comes first
            _startLevels = startLevels.OrderBy(s => s.Temperature).ToArray();
            _timer = new Timer();
            _timer.AutoReset = false;
            _timer.Elapsed += scheduleTimerElapsedHandler;
        }
        #endregion

        #region Event triggered methods
        public void UpdateSchedule(int hour, int minute, DateTime scheduleUpdated)
        {
            lock (_locker)
            {
                _hour = hour;
                _hourIsSet = true;
                _minute = minute;
                _minuteIsSet = true;

                _scheduleUpdated = scheduleUpdated;

                //Persist schedule to settings if schedule is only controlled with webapi
                if (_onlyUseWebApiToUpdateSchedule)
                {
                    PersistSchedule();
                }
            }

            ResetTimer();
        }

        public void UpdateCurrentTemperature(float currentTemperature, DateTime currentTemperatureUpdated)
        {
            lock (_locker)
            {
                _currentTemperature = currentTemperature;
                _currentTemperatureUpdated = currentTemperatureUpdated;
            }

            ResetTimer();
        }

        public void OnEndOfExecution()
        {
            _lastExecutionEndedAt = DateTime.Now;
            ResetTimer();
        }

        public void Inactivate()
        {
            lock (_locker)
            {
                _timer.Stop();

                //Persist schedule to settings if schedule is only controlled with webapi
                if (_onlyUseWebApiToUpdateSchedule)
                {
                    PersistSchedule();
                }
            }
        }
        #endregion

        #region Scheduling logic
        /// <summary>
        /// Calculates the time left to the next start time according to current conditions (temperature and next scheduled time to leave) and restarts the timer
        /// </summary>
        private void ResetTimer()
        {
            if(_currentTemperatureUpdated > DateTime.MinValue && _hourIsSet && _minuteIsSet)
            {
                lock (_locker)
                {
                    _timer.Stop();
                    TimeSpan? timeLeftToNextStart = GetTimeLeftToStartLevel();

                    //The current temperature is higher than the highest start level which means the execution should be stopped until the conditions change
                    if(timeLeftToNextStart == null)
                    {
                        Trace.WriteLine("[TemperatureWatcher][Execution][ScheduleHandler][ResetTimer] The timer has been disabled since there are no start levels with a temperature higher or equal to the current temperature");
                        return;
                    }

                    _timer.Interval = timeLeftToNextStart.Value.TotalMilliseconds;
                    Trace.WriteLine("[TemperatureWatcher][Execution][ScheduleHandler][ResetTimer] The timer has a new value, executable will run in " + _timer.Interval + " milliseconds");
                    _timer.Start();
                }
            }
        }

        public StartLevel GetCurrentStartLevel()
        {
            //Returns null if the current temperature is warmer than the start level with the highest temperature
            StartLevel currentLevel = null;

            //Find current start level depending on the current temperature
            foreach (StartLevel s in _startLevels)
            {
                if (_currentTemperature <= s.Temperature)
                {
                    currentLevel = s;
                    break;
                }
            }

            return currentLevel;
        }

        private TimeSpan? GetTimeLeftToStartLevel()
        {
            lock (_locker)
            {
                StartLevel currentLevel = GetCurrentStartLevel();

                //Current temperature is warmer than the start level with the highest temperature
                if(currentLevel == null)
                {
                    return null;
                }

                //Set now to a variable so that it does not change inside the method
                DateTime now = DateTime.Now;

                //Get next leave time as datetime
                DateTime nextStartTime = GetNextScheduledTime(now);

                //Remove the start level time settings from the next start time so that it is set to correct time before leave time
                nextStartTime = nextStartTime.AddHours(-currentLevel.Hours);
                nextStartTime = nextStartTime.AddMinutes(-currentLevel.Minutes);
                nextStartTime = nextStartTime.AddSeconds(-currentLevel.Seconds);

                /* If the last execution ended after the calculated nextStart time then add a day. This happens
                 * for example when the execution is prematurely stopped by a WebApi request and the temperature
                 * or scheduled time to leave is updated after that (which causes the timer to be reset and this
                 * method to be called) */
                if(_lastExecutionEndedAt >= nextStartTime)
                {
                    nextStartTime = nextStartTime.AddDays(1);
                }

                TimeSpan timeLeftToStart = nextStartTime - now;
                //Return timespan for when to start execution
                if (timeLeftToStart.TotalMilliseconds >= 0)
                {
                    return nextStartTime - now;
                }
                /* Start instantly, i.e. starttime is already passed but leave time is not, the service was either 
                 * started at a time when execution should already be started or else the temperature or scheduled
                 * time to leave was changed so that the service should start immediately */
                else
                {
                    return new TimeSpan(1);
                }
            }
        }

        /// <summary>
        /// Get the next scheduled time from the specified time reference
        /// </summary>
        /// <param name="now">Time reference from which to get next schedued time</param>
        /// <param name="incrementDayIfPassed">If true, increments the day if scheduled time has passed the reference time, if false, does not increment the day</param>
        /// <returns></returns>
        public DateTime GetNextScheduledTime(DateTime now, bool incrementDayIfPassed = true)
        {
            //Set to today and then add hour and minute from the leave time source
            DateTime nextStartTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            nextStartTime = nextStartTime.AddHours(_hour);
            nextStartTime = nextStartTime.AddMinutes(_minute);

            //Increment day if leavetime is passed (will occur next day)
            if (incrementDayIfPassed && now > nextStartTime)
            {
                nextStartTime = nextStartTime.AddDays(1);
            }

            return nextStartTime;
        }
        #endregion

        #region Schedule persistance
        /// <summary>
        /// Persist schedules settings to settings file
        /// </summary>
        private void PersistSchedule()
        {
            Properties.Settings.Default.Hour = Hour;
            Properties.Settings.Default.Minute = Minute;
            Properties.Settings.Default.IsActive = IsActive;
            Properties.Settings.Default.Save();
        }
        #endregion
    }
}
