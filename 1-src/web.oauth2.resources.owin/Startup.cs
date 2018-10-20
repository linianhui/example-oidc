using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(Web.OAuth2.Resources.Startup))]

namespace Web.OAuth2.Resources
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "http://oidc-server.test",
                ValidationMode = ValidationMode.Both,
                RequireHttps = false,
                TokenProvider = new OAuthBearerAuthenticationProvider
                {
                    OnRequestToken = HandlerRequestToken,
                    OnValidateIdentity = HandlerValidateIdentity,
                    OnApplyChallenge = HandlerApplyChallenge,
                }
            });

            app.UseWebApi(GetWebApiConfig());
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task HandlerRequestToken(OAuthRequestTokenContext context)
        {
            //默认从context.Request.Headers["Authorization"]获取
            var token = context.Token;
            return Task.FromResult(0);
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task HandlerValidateIdentity(OAuthValidateIdentityContext context)
        {
            //处理一些额外的验证逻辑
            //失败后调用context.Rejected();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Token验证失败
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task HandlerApplyChallenge(OAuthChallengeContext context)
        {
            //默认返回401
            return Task.FromResult(0);
        }

        private static HttpConfiguration GetWebApiConfig()
        {
            var config = new HttpConfiguration();

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

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