using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TemperatureWatcher.Configuration;

namespace TemperatureWatcher.WebApi.Controllers
{
    public class AuthUsedController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(Config.GetInstance().WebApi.UseAuth);
        }
    }
}
