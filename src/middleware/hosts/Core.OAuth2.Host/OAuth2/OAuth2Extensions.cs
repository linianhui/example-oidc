using Microsoft.AspNetCore.Builder;
using OAuth2.QQConnect;

namespace Core.OAuth2.Host.OAuth2
{
    public static class OAuth2Extensions
    {
        public static IApplicationBuilder UseQQConnect(this IApplicationBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = OAuth2Constants.AuthenticationSchemeOfCookie
            });

            return app.UseQQConnectAuthentication(new QQConnectOptions
            {
                AuthenticationScheme = OAuth2Constants.AuthenticationSchemeOfQQ,
                SignInScheme = OAuth2Constants.AuthenticationSchemeOfCookie,
                ClientId = "You App Id",
                ClientSecret = "You App Secret"
            });
        }
    }
}