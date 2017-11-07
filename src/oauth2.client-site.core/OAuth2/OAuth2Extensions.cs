using Microsoft.AspNetCore.Builder;
using OAuth2.QQConnect;
using OAuth2.QQConnect.Core1;

namespace ClientSite.OAuth2
{
    public static class OAuth2Extensions
    {
        public static IApplicationBuilder UseQQConnect(this IApplicationBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = OAuth2Constants.AuthenticationSchemeOfCookie
            });

            return app.UseQQConnectAuthentication(new CoreQQConnectOptions
            {
                AuthenticationScheme = OAuth2Constants.AuthenticationSchemeOfQQ,
                SignInScheme = OAuth2Constants.AuthenticationSchemeOfCookie,
                ClientId = QQConnectConfig.ClientId,
                ClientSecret = QQConnectConfig.ClientSecret
            });
        }
    }
}