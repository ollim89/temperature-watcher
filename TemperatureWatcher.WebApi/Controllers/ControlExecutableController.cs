using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TemperatureWatcher.Common.WebApiEventTypes;
using TemperatureWatcher.Common.WebApiResponseTypes;
using TemperatureWatcher.WebApi.Models;

namespace TemperatureWatcher.WebApi.Controllers
{
    public class ControlExecutableController : ApiControllerBase
    {
        public IHttpActionResult Post([FromBody]ExecutableControl executableControl)
        {
            TemperatureWatcherExecutorCallback(new ControlExecutableEvent(executableControl.SendOnFlags, executableControl.MinutesToKeepRunning));
            return Ok();
        }
    }
}
