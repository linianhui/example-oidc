using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OAuth2.QQConnect.AspNetCore;
using OAuth2.Github.AspNetCore;

namespace ServerSite.Ids4
{
    public static class Ids4Extensions
    {
        public static IServiceCollection AddIds4(this IServiceCollection @this)
        {
            @this
                .AddAuthentication()
                .AddQQConnect("qq", "QQ Connect", SetQQConnectOptions)
                .AddGithub("github", "Github", SetGithubOptions);

            @this
                .AddIdentityServer(SetIdentityServerOptions)
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Resources.AllIdentityResources)
                .AddInMemoryApiResources(Resources.AllApiResources)
                .AddInMemoryClients(Clients.All)
                .AddTestUsers(Users.All);

            return @this;
        }

        public static IApplicationBuilder UseIds4(this IApplicationBuilder @this)
        {
            return @this.UseIdentityServer();
        }

        private static void SetQQConnectOptions(QQConnectOAuthOptions options)
        {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            options.ClientId = GlobalConfig.QQConnect.ClientId;
            options.ClientSecret = GlobalConfig.QQConnect.ClientSecret;
        }

        private static void SetGithubOptions(GithubOAuthOptions options)
        {
            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            options.ClientId = GlobalConfig.Github.ClientId;
            options.ClientSecret = GlobalConfig.Github.ClientSecret;
        }

        private static void SetIdentityServerOptions(IdentityServerOptions options)
        {
            options.IssuerUri = "http://oidc-server.test";
            options.UserInteraction = new UserInteractionOptions
            {
                LoginUrl = "/account/login",
                LoginReturnUrlParameter = "resumeUrl",
                LogoutUrl = "/account/logout",
                LogoutIdParameter = "logoutId",
                ErrorUrl = "/ids4/error",
                ErrorIdParameter = "errorId"
            };
        }
    }
}
