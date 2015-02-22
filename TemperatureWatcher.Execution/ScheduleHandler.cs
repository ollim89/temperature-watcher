using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TemperatureWatcher.Configuration.StartLevelsSection;
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
            set
            {
                lock (_locker)
                {
                    _currentTemperature = value;
                    _currentTemperatureUpdated = DateTime.Now;
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
            set
            {
                lock (_locker)
                {
                    _currentTemperatureUpdated = value;
                }
            }
        }
        #endregion

        public ScheduleHandler(StartLevelCollection startLevelCollection, ElapsedEventHandler scheduleTimerElapsedHandler)
        {
            //Initialize values
            _hourIsSet = false;
            _minuteIsSet = false;
            _currentTemperatureUpdated = DateTime.MinValue;

            StartLevel[] startLevels = new StartLevel[startLevelCollection.Count];
            for(int i = 0; i < startLevelCollection.Count; i++)
            {
                startLevels[i] = startLevelCollection[i];
            }

            //Sort start levels by temperature so that the coldest comes first
            _startLevels = startLevels.OrderBy(s => s.Temperature).ToArray();
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += scheduleTimerElapsedHandler;
        }

        public void UpdateSchedule(int hour, int minute, DateTime scheduleUpdated)
        {
            lock (_locker)
            {
                _hour = hour;
                _hourIsSet = true;
                _minute = minute;
                _minuteIsSet = true;

                _scheduleUpdated = scheduleUpdated;
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

        public void ResetTimer()
        {
            if(_currentTemperatureUpdated > DateTime.MinValue && _hourIsSet && _minuteIsSet)
            {
                lock (_locker)
                {
                    _timer.Stop();
                    _timer.Interval = GetTimeLeftToStartLevel().TotalMilliseconds;
                    _timer.Start();
                }
            }
        }

        public void Inactivate()
        {
            lock (_locker)
            {
                _timer.Stop();
            }
        }

        private TimeSpan GetTimeLeftToStartLevel()
        {
            /* Set the current level to the last to ensure that it is selected if the current temperature is greater than
             * the warmest start level */
            StartLevel currentLevel = _startLevels.Last();

            //Find current start level depending on the current temperature
            foreach (StartLevel s in _startLevels)
            {
                if (_currentTemperature <= s.Temperature)
                {
                    currentLevel = s;
                    break;
                }
            }

            //Set now to a variable so that it does not change inside the method
            DateTime now = DateTime.Now;

            //Get next leave time as datetime
            DateTime nextStartTime = GetNextScheduledTime(now);

            //Remove the start level time settings from the next start time so that it is set to correct time before leave time
            nextStartTime = nextStartTime.AddHours(-currentLevel.Hours);
            nextStartTime = nextStartTime.AddMinutes(-currentLevel.Minutes);
            nextStartTime = nextStartTime.AddSeconds(-currentLevel.Seconds);

            TimeSpan timeLeftToStart = nextStartTime - now;
            if (timeLeftToStart.TotalMilliseconds >= 0)
            {
                return nextStartTime - now;
            }
            else
            {
                return new TimeSpan(0);
            }
        }

        public DateTime GetNextScheduledTime(DateTime? now = null)
        {
            if(!now.HasValue)
            {
                now = DateTime.Now;
            }

            //Set to today and then add hour and minute from the leave time source
            DateTime nextStartTime = new DateTime(now.Value.Year, now.Value.Month, now.Value.Day, 0, 0, 0);
            nextStartTime = nextStartTime.AddHours(_hour);
            nextStartTime = nextStartTime.AddMinutes(_minute);

            //Increment day if leavetime is passed (will occur next day)
            if (now > nextStartTime)
            {
                nextStartTime = nextStartTime.AddDays(1);
            }

            return nextStartTime;
        }
    }
}
