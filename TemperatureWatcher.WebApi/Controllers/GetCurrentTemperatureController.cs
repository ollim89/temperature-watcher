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
    [AuthorizeIfAuthenticationIsUsed]
    public class GetCurrentTemperatureController : ApiControllerBase
    {
        public IHttpActionResult Get()
        {
            GetCurrentTemperatureEvent getCurrentTemperatureEvent = new GetCurrentTemperatureEvent();
            GetCurrentTemperatureResponse response = (GetCurrentTemperatureResponse)TemperatureWatcherExecutorCallback(getCurrentTemperatureEvent);
            return Ok(response);
        }
    }
}
