using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Common.WebApiResponseTypes
{
    public class GetExecutingStateResponse : IWebApiResponse
    {
        public bool IsExecuting { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool IsActive { get; set; }

        public GetExecutingStateResponse(bool isExecuting, int hour, int minute, bool isActive)
        {
            IsExecuting = isExecuting;
            Hour = hour;
            Minute = minute;
            IsActive = isActive;
        }
    }
}
