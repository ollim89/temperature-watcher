using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TemperatureWatcher.Common.WebApiEventTypes;
using TemperatureWatcher.WebApi.Models;

namespace TemperatureWatcher.WebApi.Controllers
{
    [AuthorizeIfAuthenticationIsUsed]
    public class SetScheduleController : ApiControllerBase
    {
        public IHttpActionResult Post([FromBody]Schedule schedule)
        {
            SetScheduleEvent scheduleEvent = new SetScheduleEvent(schedule.Hour, schedule.Minute, schedule.ActiveState);
            TemperatureWatcherExecutorCallback(scheduleEvent);
            return Ok();
        }
    }
}
