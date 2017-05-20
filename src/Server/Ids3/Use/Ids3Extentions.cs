using System.Web.Helpers;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using OAuth2.QQConnect;
using Owin;
using Constants = IdentityServer3.Core.Constants;

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
                    },
                    AuthenticationOptions = new AuthenticationOptions
                    {
                        //退出登录后直接重定向到Client的RedirectUrl.
                        EnablePostSignOutAutoRedirect = true,
                        IdentityProviders = ConfigureIdentityProviders
                    }
                });
            });
        }

        private static IdentityServerServiceFactory BuildIdentityServerServiceFactory()
        {
            var serviceFactory = new IdentityServerServiceFactory()
                .UseInMemoryClients(Clients.All)
                .UseInMemoryScopes(StandardScopes.All);

            serviceFactory.UserService = new Registration<IUserService, UserService>();

            return serviceFactory;
        }

        private static void ConfigureIdentityProviders(IAppBuilder app, string signInAsAuthenticationType)
        {
            app.UseQQConnectAuthentication(new QQConnectAuthenticationOptions
            {
                Caption = "QQ",
                SignInAsAuthenticationType = signInAsAuthenticationType,
                AppId = "You App Id",
                AppSecret = "You App Secret"
            });
        }

    }
}