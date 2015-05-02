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
        private Config _settings;

        public Initializer(Config settings, Func<IWebApiEvent, IWebApiResponse> callback)
        {
            ApiControllerBase.TemperatureWatcherExecutorCallback = callback;
            _settings = settings;
            Trace.WriteLine("[TemperatureWatcher][WebApi][Initializer][Constructor] WebApi will use following url: " + _settings.WebApi.Url);
            WebApp.Start<Startup>(_settings.WebApi.Url);
        }
    }
}
