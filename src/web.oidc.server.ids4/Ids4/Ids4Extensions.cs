using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OAuth2.QQConnect.AspNetCore;

namespace ServerSite.Ids4
{
    public static class Ids4Extensions
    {
        public static void AddIds4(this IServiceCollection services)
        {

            services.AddAuthentication()
                .AddQQConnect(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = QQConnectConfig.ClientId;
                    options.ClientSecret = QQConnectConfig.ClientSecret;
                },"qq");
            services
                .AddIdentityServer(SetIdentityServerOptions)
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Resources.AllIdentityResources)
                .AddInMemoryClients(Clients.All)
                .AddTestUsers(Users.All);
        }

        private static void SetIdentityServerOptions(IdentityServerOptions options)
        {
            options.IssuerUri = "http://oidc-server.dev";
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

        
        public static void UseIds4(this IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}
