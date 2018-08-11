using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using OAuth2.QQConnect.AspNetCore;

namespace ClientSite.OAuth2
{
    public static class OAuth2Extensions
    {
        public static AuthenticationBuilder AddQQConnect(this IServiceCollection services)
        {
            return services.AddAuthentication(OAuth2Constants.AuthenticationSchemeOfCookie)
                .AddQQConnectAuthentication(options =>
                  {
                      options.SignInScheme = OAuth2Constants.AuthenticationSchemeOfCookie;
                      options.ClientId = QQConnectConfig.ClientId;
                      options.ClientSecret = QQConnectConfig.ClientSecret;
                  }, scheme: OAuth2Constants.AuthenticationSchemeOfQQ);
        }
    }
}