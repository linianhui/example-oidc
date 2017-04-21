using System.Collections.Generic;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using Owin;

namespace Server.Ids3.Use
{
    public static class Ids3Extentions
    {
        public static void UseIds3(this IAppBuilder app)
        {
            app.Map("/identity", idsApp =>
            {
                idsApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Ids3 Server Web Site",
                    IssuerUri = "http://demo.server.ids3",
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
                .UseInMemoryClients(new Client[0])
                .UseInMemoryUsers(new List<InMemoryUser>())
                .UseInMemoryScopes(StandardScopes.All);
        }
    }
}