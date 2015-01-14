using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TemperatureWatcher.ConfigurationSection.StartLevelsSection;
using TemperatureWatcher.Execution.Workers;

namespace TemperatureWatcher.Execution
{
    class ScheduleHandler
    {
        private StartLevel[] _startLevels;
        private int _hour;
        private int _minute;
        private DateTime _scheduleUpdated;
        private Timer _timer;
        private float _currentTemperature;
        private DateTime _currentTemperatureUpdated;

        #region Properties
        public float CurrentTemperature
        {
            get
            {
                return _currentTemperature;
            }
            set
            {
                _currentTemperature = value;
            }
        }

        public DateTime CurrentTemperatureUpdated
        {
            get
            {
                return _currentTemperatureUpdated;
            }
            set
            {
                _currentTemperatureUpdated = value;
            }
        }
        #endregion

        public ScheduleHandler(StartLevel[] startLevels, ElapsedEventHandler scheduleTimerElapsedHandler)
        {
            //Sort start levels by temperature so that the coldest comes first
            _startLevels = startLevels.OrderBy(s => s.Temperature).ToArray();
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += scheduleTimerElapsedHandler;
        }

        public void UpdateSchedule(int hour, int minute, DateTime scheduleUpdated)
        {
            _hour = hour;
            _minute = minute;
            _scheduleUpdated = scheduleUpdated;

            ResetTimer();
        }

        public void UpdateCurrentTemperature(float currentTemperature, DateTime currentTemperatureUpdated)
        {
            _currentTemperature = currentTemperature;
            _currentTemperatureUpdated = currentTemperatureUpdated;

            ResetTimer();
        }

        public void ResetTimer()
        {
            if(_currentTemperature != null && _hour != null && _minute != null)
            {
                _timer.Stop();
                _timer.Interval = GetTimeLeftToStartLevel().Milliseconds;
                _timer.Start();
            }
        }

        public void Inactivate()
        {
            _timer.Stop();
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

            return nextStartTime - now;
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
