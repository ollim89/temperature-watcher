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
    public class ControlExecutableController : ApiControllerBase
    {
        public IHttpActionResult Post(bool sendOnFlags)
        {
            TemperatureWatcherExecutorCallback(new ControlExecutableEvent(sendOnFlags));
            return Ok();
        }
    }
}
