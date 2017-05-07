using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Web.Helpers;
using Client.Implicit.Oidc.Cleartext;
using Microsoft.Owin.Security.Cookies;
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
                TicketDataFormat = new AuthenticationTicketCleartextDataFormat()//方便调试，明文。
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = Constants.AuthenticationTypeOfOidc,
                Authority = "http://server.ids3.dev/auth",
                ClientId = "implicit-client",
                RedirectUri = "http://client.implicit.dev/",
                ResponseType = "id_token",
                SignInAsAuthenticationType = Constants.AuthenticationTypeOfCookies,
                StateDataFormat = new AuthenticationPropertiesCleartextDataFormat()//方便调试，明文。
            });
        }
    }
}