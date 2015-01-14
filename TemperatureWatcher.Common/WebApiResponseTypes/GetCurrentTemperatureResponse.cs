using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Common.WebApiResponseTypes
{
    public class GetCurrentTemperatureResponse : IWebApiResponse
    {
        public float Temperature { get; set; }
        public DateTime TemperatureUpdated { get; set; }

        public GetCurrentTemperatureResponse() { }

        public GetCurrentTemperatureResponse(float temperature, DateTime temperatureUpdated)
        {
            Temperature = temperature;
            TemperatureUpdated = temperatureUpdated;
        }
    }
}
