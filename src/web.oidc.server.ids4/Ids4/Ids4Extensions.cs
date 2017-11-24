using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ServerSite.Ids4
{
    public static class Ids4Extensions
    {
        public static void AddIds4(this IServiceCollection services)
        {
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
