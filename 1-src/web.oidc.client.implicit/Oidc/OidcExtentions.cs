using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace ClientSite.Oidc
{
    public static class OidcExtentions
    {
        public static void UseOidcAuthentication(this IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.AuthenticationTypeOfCookies,
                CookieHttpOnly = true,
                CookieName = Constants.CookieName,
                // 方便调试，明文。
                //TicketDataFormat = new AuthenticationTicketCleartextDataFormat()
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = Constants.AuthenticationTypeOfOidc,
                Authority = "http://oidc-server.test",
                ClientId = "oidc-client-implicit.test",
                ResponseType = "id_token",
                RequireHttpsMetadata = false,
                CallbackPath = new PathString("/oidc/login-callback"),
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
            context.ProtocolMessage.RedirectUri = context.Request.Uri.GetLeftPart(UriPartial.Authority);
            if (context.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
            {
                var idToken = context.OwinContext.Authentication.User.FindFirst(Constants.ClaimTypes.IdToken);
                if (idToken != null)
                {
                    context.ProtocolMessage.IdTokenHint = idToken.Value;
                }
            }
            if (context.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
            {
                context.ProtocolMessage.RedirectUri += context.Options.CallbackPath.ToString();
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