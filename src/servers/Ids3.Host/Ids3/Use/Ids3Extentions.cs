using System.Web.Helpers;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using OAuth2.QQConnect;
using Owin;
using Constants = IdentityServer3.Core.Constants;

namespace Ids3.Host.Ids3.Use
{
    public static class Ids3Extentions
    {
        public static void UseIds3(this IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            app.Map($"/{Ids3Constants.ServerPath}", idsApp =>
            {
                idsApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = Ids3Constants.SiteName,
                    IssuerUri = Ids3Constants.IssuerUri,
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
                AuthenticationType = Ids3Constants.QQIdp,
                SignInAsAuthenticationType = signInAsAuthenticationType,
                AppId = "You App Id",
                AppSecret = "You App Secret"
            });
        }

    }
}