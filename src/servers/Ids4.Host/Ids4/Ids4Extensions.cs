using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ids4.Host.Ids4
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
