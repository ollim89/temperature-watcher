using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using Microsoft.Owin.Security.OAuth;
using TemperatureWatcher.Configuration;

[assembly: OwinStartup(typeof(TemperatureWatcher.WebApi.Startup))]

namespace TemperatureWatcher.WebApi
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Trace.WriteLine("[TemperatureWatcher][WebApi][Startup][Configuration] Configuring WebApi");
            HttpConfiguration config = new HttpConfiguration();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            
            config.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            Trace.WriteLine("[TemperatureWatcher][WebApi][Startup][Configuration] Applying setings for WebApi");
            app.UseWebApi(config); 
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            if (Config.GetInstance().WebApi.UseAuth)
            {
                OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
                {
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/token"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                    Provider = new AuthorizationProvider()
                };

                app.UseOAuthAuthorizationServer(options);
                app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            }
        }
    }
}
