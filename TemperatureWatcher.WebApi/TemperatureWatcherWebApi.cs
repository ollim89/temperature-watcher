using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.Common;
using TemperatureWatcher.ConfigurationSection;

namespace TemperatureWatcher.WebApi
{
    public class TemperatureWatcherWebApi
    {
        private TemperatureWatcherSettings _settings;

        public TemperatureWatcherWebApi(TemperatureWatcherSettings settings, Func<IWebApiEvent, IWebApiResponse> callback)
        {
            ApiControllerBase.TemperatureWatcherExecutorCallback = callback;
            _settings = settings;
            WebApp.Start<Startup>(_settings.WebApi.Url);
        }
    }
}
