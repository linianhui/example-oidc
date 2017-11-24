using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ClientSite.Oidc
{
    public static class OidcExtentions
    {
        public static void AddOidcAuthentication(this IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = Constants.AuthenticationSchemeOfCookies;
                options.DefaultChallengeScheme = Constants.AuthenticationSchemeOfOidc;
            })
            .AddCookie(Constants.AuthenticationSchemeOfCookies, _ =>
            {
                _.Cookie.Name = Constants.CookieName;
            })
            .AddOpenIdConnect(Constants.AuthenticationSchemeOfOidc, _ =>
            {
                _.SignInScheme = Constants.AuthenticationSchemeOfCookies;
                _.Authority = "http://oidc-server.dev";
                _.RequireHttpsMetadata = false;
                _.ClientId = "authorization-code-client";
                _.ClientSecret = "lnh";
                _.ResponseType = "code id_token";
                _.SaveTokens = true;
                _.GetClaimsFromUserInfoEndpoint = false;
                _.CallbackPath = new PathString("/oidc/signin-callback");
                _.SignedOutCallbackPath = new PathString("/oidc/signout-callback");
                _.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = OnRedirectToIdentityProvider
                };
            });
        }

        private static Task OnRedirectToIdentityProvider(RedirectContext redirectContext)
        {
            if (redirectContext.HttpContext.Items.ContainsKey("idp"))
            {
                var idp = redirectContext.HttpContext.Items["idp"];
                redirectContext.ProtocolMessage.AcrValues = "idp:" + idp;
            }

            return Task.FromResult(0);
        }
    }
}