using Microsoft.Owin.Security.Cookies;
using OAuth2.QQConnect;
using OAuth2.QQConnect.Owin;
using Owin;

namespace ClientSite.OAuth2
{
    public static class OAuth2Extensions
    {
        public static IAppBuilder UseQQConnect(this IAppBuilder @this)
        {
            return @this
                .UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = OAuth2Constants.AuthenticationTypeOfCookie
                })
                .UseQQConnectAuthentication(new QQConnectOAuthOptions
                {
                    AuthenticationType = OAuth2Constants.AuthenticationTypeOfQQ,
                    SignInAsAuthenticationType = OAuth2Constants.AuthenticationTypeOfCookie,
                    ClientId = GlobalConfig.QQConnect.ClientId,
                    ClientSecret = GlobalConfig.QQConnect.ClientSecret
                });
        }
    }
}