using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using System;
using System.Net.Http;

namespace OAuth2.QQConnect
{
    public class QQConnectMiddleware : AuthenticationMiddleware<QQConnectOptions>
    {
        private readonly ILogger _logger;

        public QQConnectMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            QQConnectOptions options)
            : base(next, options)
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
                var dataProtector = app.CreateDataProtector(typeof(QQConnectMiddleware).FullName, Options.AuthenticationType, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            _logger = app.CreateLogger<QQConnectMiddleware>();
        }

        protected override AuthenticationHandler<QQConnectOptions> CreateHandler()
        {
            return new QQConnectHandler(_logger, new HttpClient());
        }
    }
}