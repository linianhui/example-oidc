using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using System;
using System.Net.Http;

namespace OAuth2.QQConnect.Owin
{
    public class QQConnectOAuthMiddleware : AuthenticationMiddleware<QQConnectOAuthOptions>
    {
        private readonly ILogger _logger;

        public QQConnectOAuthMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            QQConnectOAuthOptions oAuthOptions)
            : base(next, oAuthOptions)
        {
            if (string.IsNullOrWhiteSpace(Options.ClientId))
            {
                throw new ArgumentNullException(nameof(Options.ClientId));
            }
            if (string.IsNullOrWhiteSpace(Options.ClientSecret))
            {
                throw new ArgumentNullException(nameof(Options.ClientSecret));
            }

            if (string.IsNullOrEmpty(Options.SignInAsAuthenticationType))
            {
                Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
            }

            if (Options.StateDataFormat == null)
            {
                var dataProtector = app.CreateDataProtector(typeof(QQConnectOAuthMiddleware).FullName, Options.AuthenticationType, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            _logger = app.CreateLogger<QQConnectOAuthMiddleware>();
        }

        protected override AuthenticationHandler<QQConnectOAuthOptions> CreateHandler()
        {
            return new QQConnectOAuthHandler(_logger, new HttpClient());
        }
    }
}