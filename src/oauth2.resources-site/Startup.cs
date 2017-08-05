using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(OAuth2.Resources.Startup))]

namespace OAuth2.Resources
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(GetWebApiConfig());
        }

        public static HttpConfiguration GetWebApiConfig()
        {
            var config = new HttpConfiguration();

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}",
                defaults: new { id = RouteParameter.Optional }
            );

            return config;
        }
    }
}