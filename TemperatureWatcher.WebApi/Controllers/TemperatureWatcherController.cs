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

        public void SetLeaveTime(int hour, int minute, bool activeState)
        {
            SetLeaveTimeEvent leaveTimeEvent = new SetLeaveTimeEvent(hour, minute, activeState);
            TemperatureWatcherExecutorCallback(leaveTimeEvent);
        }
    }
}
