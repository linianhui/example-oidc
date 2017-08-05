using System.IdentityModel.Tokens;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(OAuth2.Resources.Startup))]

namespace OAuth2.Resources
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap.Clear();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://server.ids3.dev/auth",
                ValidationMode = ValidationMode.Both
            });

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