using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2.QQConnect
{
    internal class QQConnectAuthenticationHandler : AuthenticationHandler<QQConnectAuthenticationOptions>
    {
        private readonly ILogger _logger;

        public QQConnectAuthenticationHandler(ILogger logger)
        {
            _logger = logger;
        }

        public override async Task<bool> InvokeAsync()
        {
            if (Options.CallbackPath == Request.Path.Value)
            {
                var ticket = await AuthenticateAsync();
                if (ticket == null)
                {
                    _logger.WriteWarning("Invalid return state, unable to redirect.");
                    Response.StatusCode = 500;
                    return true;
                }
            }
            return false;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            AuthenticationProperties properties = null;

            try
            {
                var code = Request.Query.Get("code");
                var state = Request.Query.Get("state");

                properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return null;
                }

                if (!ValidateCorrelationId(properties, _logger))
                {
                    return new AuthenticationTicket(null, properties);
                }

                if (code == null)
                {
                    return new AuthenticationTicket(null, properties);
                }

                _logger.WriteInformation(code);

                return null;
            }
            catch (Exception ex)
            {
                _logger.WriteError("Authentication failed", ex);
                return new AuthenticationTicket(null, properties);
            }
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode != 401)
            {
                return Task.FromResult<object>(null);
            }

            var authenticationChallenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

            if (authenticationChallenge != null)
            {
                var authenticationProperties = authenticationChallenge.Properties;
                if (string.IsNullOrWhiteSpace(authenticationProperties.RedirectUri))
                {
                    authenticationProperties.RedirectUri = Request.Uri.ToString();
                }

                GenerateCorrelationId(authenticationProperties);

                var authorizationUrl = BuildAuthorizationUrl(authenticationProperties);

                base.Context.Response.Redirect(authorizationUrl);
            }

            return Task.FromResult<object>(null);
        }

        private string BuildAuthorizationUrl(AuthenticationProperties authenticationProperties)
        {
            var state = Options.StateDataFormat.Protect(authenticationProperties);

            var authorizationUrlBuilder = new StringBuilder(Options.AuthorizationEndpoint);

            authorizationUrlBuilder.Append("?response_type=code");
            authorizationUrlBuilder.Append($"&client_id={Uri.EscapeDataString(Options.AppId)}");
            authorizationUrlBuilder.Append($"&redirect_uri={Uri.EscapeDataString(RedirectUri)}");
            authorizationUrlBuilder.Append($"&state={Uri.EscapeDataString(state)}");

            if (Options.Scopes != null && Options.Scopes.Count > 0)
            {
                var scope = string.Join(",", Options.Scopes);
                authorizationUrlBuilder.Append($"&scope={Uri.EscapeDataString(scope)}");
            }

            if (string.IsNullOrWhiteSpace(Options.DisplayMode) == false)
            {
                authorizationUrlBuilder.Append($"&display={Uri.EscapeDataString(Options.DisplayMode)}");
            }

            return authorizationUrlBuilder.ToString();
        }

        private string RedirectUri => BaseUri + Options.CallbackPath;

        private string BaseUri => Request.Scheme +
                                  Uri.SchemeDelimiter +
                                  Request.Host +
                                  Request.PathBase;
    }
}