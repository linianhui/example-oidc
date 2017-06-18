using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OAuth2.QQConnect;

namespace ServerSite.Ids4
{
    public static class Ids4Extensions
    {
        public static void ConfigIds4(this IServiceCollection services)
        {
            services
                .AddIdentityServer(SetIdentityServerOptions)
                .AddTemporarySigningCredential()
                .AddInMemoryIdentityResources(Resources.AllIdentityResources)
                .AddInMemoryClients(Clients.All)
                .AddTestUsers(Users.All);
        }

        private static void SetIdentityServerOptions(IdentityServerOptions options)
        {
            options.IssuerUri = "http://server.ids4.dev";
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

            app.UseQQConnectAuthentication(new QQConnectOptions
            {
                AuthenticationScheme = "qq",
                DisplayName = "QQ",
                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                ClientId = "You App Id",
                ClientSecret = "You App Secret"
            });
        }
    }
}
