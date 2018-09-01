using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using OAuth2.Github.AspNetCore;
using OAuth2.QQConnect.AspNetCore;

namespace ClientSite.OAuth2
{
    public static class OAuth2Extensions
    {
        public static AuthenticationBuilder AddQQConnect(this IServiceCollection @this)
        {
            return @this
                .AddAuthentication(OAuth2Constants.AuthenticationSchemeOfCookie)
                .AddCookie(OAuth2Constants.AuthenticationSchemeOfCookie)
                .AddQQConnect(OAuth2Constants.AuthenticationSchemeOfQQ, "QQ Connect", SetQQConnectOptions)
                .AddGithub(OAuth2Constants.AuthenticationSchemeOfGithub, "Github", SetGithubOptions);
        }

        private static void SetQQConnectOptions(QQConnectOAuthOptions options)
        {
            options.SignInScheme = OAuth2Constants.AuthenticationSchemeOfCookie;
            options.ClientId = GlobalConfig.QQConnect.ClientId;
            options.ClientSecret = GlobalConfig.QQConnect.ClientSecret;
        }

        private static void SetGithubOptions(GithubOAuthOptions options)
        {
            options.SignInScheme = OAuth2Constants.AuthenticationSchemeOfCookie;
            options.ClientId = GlobalConfig.Github.ClientId;
            options.ClientSecret = GlobalConfig.Github.ClientSecret;
        }
    }
}