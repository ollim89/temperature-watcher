using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.ConfigurationSection.StartLevelsSection;
using TemperatureWatcher.Execution.Workers;

namespace TemperatureWatcher.Execution
{
    class TimeToLeaveHandler
    {
        private ExternalPathWorker _worker;
        private bool _isUsingWorker;
        private bool _isActive;
        private int _hour;
        private int _minute;

        public TimeSpan GetTimeLeftToStartLevel(float currentTemperature, StartLevel[] startLevels)
        {
            //Sort start levels by temperature so that the coldest comes first
            startLevels = startLevels.OrderBy(s => s.Temperature).ToArray();

            /* Set the current level to the last to ensure that it is selected if the current temperature is greater than
             * the warmest start level */
            StartLevel currentLevel = startLevels.Last();

            //Find current start level depending on the current temperature
            foreach(StartLevel s in startLevels)
            {
                if(currentTemperature <= s.Temperature)
                {
                    currentLevel = s;
                    break;
                }
            }

            //Set now to a variable so that it does not shift inside the method
            DateTime now = DateTime.Now;

            //Set to today and then add hour and minute from the leave time source
            DateTime nextStartTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            nextStartTime = nextStartTime.AddHours(Hour);
            nextStartTime = nextStartTime.AddMinutes(Minute);

            //Increment day if leavetime is passed (will occur next day)
            if(now > nextStartTime)
            {
                nextStartTime = nextStartTime.AddDays(1);
            }

            //Remove the start level time settings from the next start time so that it is set to correct time before leave time
            nextStartTime = nextStartTime.AddHours(-currentLevel.Hours);
            nextStartTime = nextStartTime.AddMinutes(-currentLevel.Minutes);
            nextStartTime = nextStartTime.AddSeconds(-currentLevel.Seconds);

            return nextStartTime - now;
        }

        public int Hour
        {
            get
            {
                if(_isUsingWorker)
                {
                    return GetHourFromWorker();
                }
                else
                {
                    return _hour;
                }
            }
            set
            {
                _hour = value;
            }
        }

        public int Minute
        {
            get
            {
                if (_isUsingWorker)
                {
                    return GetMinuteFromWorker();
                }
                else
                {
                    return _minute;
                }
            }
            set
            {
                _minute = value;
            }
        }

        public bool IsActive
        {
            get
            {
                if (_isUsingWorker)
                {
                    return GetIsActiveFromWorker();
                }
                else
                {
                    return _isActive;
                }
            }
            set
            {
                _isActive = value;
            }
        }

        public TimeToLeaveHandler(ExternalPathWorker worker)
        {
            _worker = worker;
            _isUsingWorker = true;
        }
        
        public TimeToLeaveHandler()
        {
            _isUsingWorker = false;
        }

        private bool GetIsActiveFromWorker()
        {
            return Convert.ToBoolean(_worker.Content.Split(';')[1]);
        }

        private int GetHourFromWorker()
        {
            return Convert.ToInt16(_worker.Content.Split(';')[0].Split(':')[0]);
        }

        private int GetMinuteFromWorker()
        {
            return Convert.ToInt16(_worker.Content.Split(';')[0].Split(':')[1]);
        }
    }
}
