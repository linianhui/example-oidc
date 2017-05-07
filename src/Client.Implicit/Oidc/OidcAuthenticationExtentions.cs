using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Client.Implicit.Oidc.Cleartext;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace Client.Implicit.Oidc
{
    public static class OidcAuthenticationExtentions
    {
        public static void UseOidcAuthentication(this IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.AuthenticationTypeOfCookies,
                CookieHttpOnly = true,
                CookieName = Constants.AuthenticationTypeOfCookies + Constants.AuthenticationTypeOfOidc,
                // 方便调试，明文。
                //TicketDataFormat = new AuthenticationTicketCleartextDataFormat()
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = Constants.AuthenticationTypeOfOidc,
                Authority = "http://server.ids3.dev/auth",
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
            return Task.FromResult(0);
        }
    }
}