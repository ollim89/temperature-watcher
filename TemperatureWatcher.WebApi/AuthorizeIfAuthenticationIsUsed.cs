using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TemperatureWatcher.Configuration;

namespace TemperatureWatcher.WebApi
{
    /// <summary>
    /// Overrides the default Authorize attribute and reads from config if auth should be used
    /// </summary>
    class AuthorizeIfAuthenticationIsUsed : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (Config.GetInstance().WebApi.UseAuth)
            {
                base.OnAuthorization(actionContext);
            }
        }

        public override Task OnAuthorizationAsync(System.Web.Http.Controllers.HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            if (Config.GetInstance().WebApi.UseAuth)
            {
                return base.OnAuthorizationAsync(actionContext, cancellationToken);
            }
            else
            {
                return Task.Run(() =>
                {

                });
            }
        }
    }
}
