using Microsoft.Owin.Security.Cookies;
using OAuth2.QQConnect;

namespace Owin.OAuth2.Host.OAuth2
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
                ClientId = "You App Id",
                ClientSecret = "You App Secret"
            });
        }
    }
}