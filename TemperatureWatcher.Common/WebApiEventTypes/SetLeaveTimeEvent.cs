using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Common.WebApiEventTypes
{
    public class SetLeaveTimeEvent : IWebApiEvent
    {
        public SetLeaveTimeEvent(int hour, int minute, bool isActive)
        {
            Hour = hour;
            Minute = minute;
            IsActive = isActive;
        }

        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool IsActive { get; set; }
    }
}
