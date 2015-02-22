using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.Common.WebApiResponseTypes
{
    public class GetExecutingStateResponse : IWebApiResponse
    {
        public bool IsExecuting { get; set; }

        public GetExecutingStateResponse(bool isExecuting)
        {
            IsExecuting = isExecuting;
        }
    }
}
