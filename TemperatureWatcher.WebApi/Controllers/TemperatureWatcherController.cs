using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TemperatureWatcher.Common.WebApiEventTypes;
using TemperatureWatcher.Common.WebApiResponseTypes;

namespace TemperatureWatcher.WebApi.Controllers
{
    public class TemperatureWatcherController : ApiControllerBase
    {
        public void StartExecutable()
        {
            TemperatureWatcherExecutorCallback(new StartExecutableEvent());
        }

        public bool GetExecutingState()
        {
            bool isExecuting = ((GetExecutingStateResponse)TemperatureWatcherExecutorCallback(new GetExecutingStateEvent())).IsExecuting;
            return isExecuting;
        }

        public void SetSchedule(int hour, int minute, bool activeState)
        {
            SetScheduleEvent scheduleEvent = new SetScheduleEvent(hour, minute, activeState);
            TemperatureWatcherExecutorCallback(scheduleEvent);
        }

        public object GetCurrentTemperature()
        {
            GetCurrentTemperatureEvent getCurrentTemperatureEvent = new GetCurrentTemperatureEvent();
            GetCurrentTemperatureResponse response = (GetCurrentTemperatureResponse)TemperatureWatcherExecutorCallback(getCurrentTemperatureEvent);
            return Json(response);
        }
    }
}
