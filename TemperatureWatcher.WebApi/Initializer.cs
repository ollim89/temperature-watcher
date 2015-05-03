using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.Common;
using TemperatureWatcher.Configuration;

namespace TemperatureWatcher.WebApi
{
    public class Initializer
    {
        public Initializer(Func<IWebApiEvent, IWebApiResponse> callback)
        {
            ApiControllerBase.TemperatureWatcherExecutorCallback = callback;
            Trace.WriteLine("[TemperatureWatcher][WebApi][Initializer][Constructor] WebApi will use following url: " + Config.GetInstance().WebApi.Url);
            WebApp.Start<Startup>(Config.GetInstance().WebApi.Url);
        }
    }
}
