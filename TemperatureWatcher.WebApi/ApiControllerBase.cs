using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TemperatureWatcher.Common;

namespace TemperatureWatcher.WebApi
{
    public class ApiControllerBase : ApiController
    {
        public static Func<IWebApiEvent, IWebApiResponse> TemperatureWatcherExecutorCallback;
    }
}
