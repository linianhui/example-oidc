using System.Collections.Generic;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace Client.Implicit.OpenIdConnect
{
    public static class OpenIdConnectClientExtentions
    {
        public static void UseOpenIdConnectClient(this IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "http://server.ids3.dev/auth",
                ClientId = "implicit-client",
                RedirectUri = "http://client.implicit.dev/",
                ResponseType = "id_token",
                SignInAsAuthenticationType = "Cookies"
            });
        }
    }
}