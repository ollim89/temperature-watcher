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
    public class GetExecutingStateController : ApiControllerBase
    {
        public IHttpActionResult Get()
        {
            GetExecutingStateResponse response = (GetExecutingStateResponse)TemperatureWatcherExecutorCallback(new GetExecutingStateEvent());
            return Ok(response);
        }
    }
}
