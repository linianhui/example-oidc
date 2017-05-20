using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using System;
using System.Net.Http;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;

namespace OAuth2.QQConnect
{
    public class QQConnectAuthenticationMiddleware : AuthenticationMiddleware<QQConnectAuthenticationOptions>
    {
        private readonly ILogger _logger;

        public QQConnectAuthenticationMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            QQConnectAuthenticationOptions options)
            : base(next, options)
        {
            if (string.IsNullOrWhiteSpace(Options.AppId))
            {
                throw new ArgumentNullException(nameof(Options.AppId));
            }
            if (string.IsNullOrWhiteSpace(Options.AppSecret))
            {
                throw new ArgumentNullException(nameof(Options.AppSecret));
            }

            if (string.IsNullOrEmpty(Options.SignInAsAuthenticationType))
            {
                Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
            }

            if (Options.StateDataFormat == null)
            {
                var dataProtector = app.CreateDataProtector(typeof(QQConnectAuthenticationMiddleware).FullName, Options.AuthenticationType, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            _logger = app.CreateLogger<QQConnectAuthenticationMiddleware>();
        }

        protected override AuthenticationHandler<QQConnectAuthenticationOptions> CreateHandler()
        {
            return new QQConnectAuthenticationHandler(_logger, new HttpClient());
        }
    }
}