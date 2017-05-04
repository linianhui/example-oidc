using System.Web.Helpers;
using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using Owin;

namespace Server.Ids3.Use
{
    public static class Ids3Extentions
    {
        public static void UseIds3(this IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            app.Map("/auth", idsApp =>
            {
                idsApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Ids3 Server Web Site",
                    IssuerUri = "http://server.ids3.dev",
                    RequireSsl = false,
                    SigningCertificate = Certificates.SigningCertificate,
                    Factory = BuildIdentityServerServiceFactory(),
                    LoggingOptions = new LoggingOptions
                    {
                        EnableWebApiDiagnostics = true
                    }
                });
            });
        }

        private static IdentityServerServiceFactory BuildIdentityServerServiceFactory()
        {
            return new IdentityServerServiceFactory()
                .UseInMemoryClients(Clients.All)
                .UseInMemoryUsers(Users.All)
                .UseInMemoryScopes(StandardScopes.All);
        }
    }
}