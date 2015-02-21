using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Common.WebApiEventTypes
{
    public class ControlExecutableEvent : IWebApiEvent
    {
        public bool SendOnFlags { get; set; }

        public ControlExecutableEvent(bool sendOnFlags)
        {
            SendOnFlags = sendOnFlags;
        }
    }
}
