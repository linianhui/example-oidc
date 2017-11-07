using System;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Owin;

namespace OAuth2.QQConnect.Owin
{
    public class OwinQQConnectMiddleware : AuthenticationMiddleware<OwinQQConnectOptions>
    {
        private readonly ILogger _logger;

        public OwinQQConnectMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            OwinQQConnectOptions options)
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
                var dataProtector = app.CreateDataProtector(typeof(OwinQQConnectMiddleware).FullName, Options.AuthenticationType, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            _logger = app.CreateLogger<OwinQQConnectMiddleware>();
        }

        protected override AuthenticationHandler<OwinQQConnectOptions> CreateHandler()
        {
            return new OwinQQConnectHandler(_logger, new HttpClient());
        }
    }
}