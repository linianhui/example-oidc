using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using OAuth2.QQConnect.Extensions;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuth2.QQConnect.Owin
{
    public class QQConnectOAuthHandler : AuthenticationHandler<QQConnectOAuthOptions>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        private QQConnectClient _innerClient;

        private QQConnectClient InnerClient
        {
            get
            {
                if (_innerClient == null)
                {
                    var qqConnectOptions = Options.BuildQQConnectOptions(GetRedirectUrl);
                    _innerClient = new QQConnectClient(_httpClient, qqConnectOptions);
                }
                return _innerClient;
            }
        }

        public QQConnectOAuthHandler(ILogger logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public override async Task<bool> InvokeAsync()
        {
            if (Options.CallbackPath != Request.Path.Value)
            {
                return false;
            }
            var ticket = await AuthenticateAsync();
            if (ticket?.Identity != null)
            {
                var identity = ticket.Identity;
                if (identity.AuthenticationType != Options.SignInAsAuthenticationType)
                {
                    identity = new ClaimsIdentity(
                        ticket.Identity.Claims,
                        Options.SignInAsAuthenticationType,
                        ticket.Identity.NameClaimType,
                        ticket.Identity.RoleClaimType);
                }

                Context.Authentication.SignIn(ticket.Properties, identity);

                Context.Response.Redirect(ticket.Properties.RedirectUri);
            }
            else
            {
                _logger.WriteError("Invalid return state, unable to redirect.");
                Response.StatusCode = 500;
            }

            return true;
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

                var token = await InnerClient.GetTokenAsync(
                    code,
                    Request.CallCancelled);

                if (string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    _logger.WriteError("access_token was not found");
                    return new AuthenticationTicket(null, properties);
                }

                var openId = await InnerClient.GetOpenIdAsync(
                    token.AccessToken,
                    Request.CallCancelled);

                if (string.IsNullOrWhiteSpace(openId.OpenId))
                {
                    _logger.WriteError("openid was not found");
                    return new AuthenticationTicket(null, properties);
                }

                var user = await InnerClient.GetUserAsync(
                    token.AccessToken,
                    openId.OpenId,
                    Request.CallCancelled);

                var qqConnectProfile = QQConnectProfile.From(Options.AuthenticationType, token, openId, user);

                var identity = qqConnectProfile.BuildClaimsIdentity();

                return new AuthenticationTicket(identity, properties);
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

                Context.Response.Redirect(authorizationUrl);
            }

            return Task.FromResult<object>(null);
        }

        private string BuildAuthorizationUrl(AuthenticationProperties authenticationProperties)
        {
            var qqConnectProperties = authenticationProperties.Dictionary.GetQQConnectProperties();
            authenticationProperties.Dictionary.RemoveQQConnectProperties();

            var state = Options.StateDataFormat.Protect(authenticationProperties);

            return InnerClient.BuildAuthorizationUrl(qqConnectProperties, state);
        }

        private string GetRedirectUrl()
        {
            return Request.Scheme +
                   Uri.SchemeDelimiter +
                   Request.Host +
                   Request.PathBase +
                   Options.CallbackPath;
        }
    }
}