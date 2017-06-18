using Microsoft.Owin.Security.Cookies;
using OAuth2.QQConnect;
using Owin;

namespace ClientSite.OAuth2
{
    public static class OAuth2Extensions
    {
        public static IAppBuilder UseQQConnect(this IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = OAuth2Constants.AuthenticationTypeOfCookie
            });

            return app.UseQQConnectAuthentication(new QQConnectOptions
            {
                Caption = "QQ",
                AuthenticationType = OAuth2Constants.AuthenticationTypeOfQQ,
                SignInAsAuthenticationType = OAuth2Constants.AuthenticationTypeOfCookie,
                ClientId = QQConnectConfig.ClientId,
                ClientSecret = QQConnectConfig.ClientSecret
            });
        }
    }
}