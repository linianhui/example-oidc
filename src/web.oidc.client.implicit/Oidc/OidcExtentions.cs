using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace ClientSite.Oidc
{
    public static class OidcExtentions
    {
        public static void UseBaseAuthentication(this IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.AuthenticationTypeOfCookies,
                CookieHttpOnly = true,
                CookieName = Constants.AuthenticationTypeOfCookies + Constants.AuthenticationTypeOfIds3,
                // 方便调试，明文。
                //TicketDataFormat = new AuthenticationTicketCleartextDataFormat()
            });
        }

        public static void UseIds3Authentication(this IAppBuilder app)
        {
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = Constants.AuthenticationTypeOfIds3,
                Authority = "http://server.oidc.dev",
                ClientId = "implicit-client",
                RedirectUri = "http://client.implicit.dev/",
                ResponseType = "id_token",
                SignInAsAuthenticationType = Constants.AuthenticationTypeOfCookies,
                //方便调试，明文。
                //StateDataFormat = new AuthenticationPropertiesCleartextDataFormat(),
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = HandleSecurityTokenValidatedNotification,
                    RedirectToIdentityProvider = HandleRedirectToIdentityProviderNotification
                }
            });
        }

        public static void UseIds4Authentication(this IAppBuilder app)
        {
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = Constants.AuthenticationTypeOfIds4,
                Authority = "http://server.ids4.dev",
                ClientId = "implicit-client",
                RedirectUri = "http://client.implicit.dev/",
                ResponseType = "id_token",
                SignInAsAuthenticationType = Constants.AuthenticationTypeOfCookies,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = HandleSecurityTokenValidatedNotification,
                    RedirectToIdentityProvider = HandleRedirectToIdentityProviderNotification
                }
            });
        }

        private static Task HandleSecurityTokenValidatedNotification(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.AuthenticationTicket.Identity.AddClaim(new Claim(Constants.ClaimTypes.IdToken, context.ProtocolMessage.IdToken));
            return Task.FromResult(0);
        }

        private static Task HandleRedirectToIdentityProviderNotification(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            if (context.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
            {
                context.ProtocolMessage.RedirectUri = context.Request.Uri.GetLeftPart(System.UriPartial.Authority);
                var idToken = context.OwinContext.Authentication.User.FindFirst(Constants.ClaimTypes.IdToken);
                if (idToken != null)
                {
                    context.ProtocolMessage.IdTokenHint = idToken.Value;
                }
            }
            if (context.ProtocolMessage.RequestType == OpenIdConnectRequestType.AuthenticationRequest)
            {
                var idp = context.OwinContext.Get<string>("idp");
                if (string.IsNullOrWhiteSpace(idp) == false)
                {
                    context.ProtocolMessage.AcrValues = "idp:" + idp;
                }
            }
            return Task.FromResult(0);
        }
    }
}