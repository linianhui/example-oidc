using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuth2.QQConnect
{
    /// <summary>
    /// <see cref="http://wiki.connect.qq.com/%e5%bc%80%e5%8f%91%e6%94%bb%e7%95%a5_server-side"/>
    /// </summary>
    public class QQConnectHandler : AuthenticationHandler<QQConnectOptions>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public QQConnectHandler(ILogger logger, HttpClient httpClient)
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
            AuthenticationProperties authenticationProperties = null;

            try
            {
                var code = Request.Query.Get("code");
                var state = Request.Query.Get("state");

                authenticationProperties = Options.StateDataFormat.Unprotect(state);
                if (authenticationProperties == null)
                {
                    return null;
                }

                if (!ValidateCorrelationId(authenticationProperties, _logger))
                {
                    return new AuthenticationTicket(null, authenticationProperties);
                }

                if (code == null)
                {
                    return new AuthenticationTicket(null, authenticationProperties);
                }

                var accessTokenResult = await GetAccessTokenResult(code);
                var accessToken = accessTokenResult[QQConnectDefaults.AccessTokenField];
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    _logger.WriteError("access_token was not found");
                    return new AuthenticationTicket(null, authenticationProperties);
                }

                var openIdResult = await GetOpenIdResult(accessToken);
                var openId = openIdResult.TryGetValue(QQConnectDefaults.OpenIdField);
                if (string.IsNullOrWhiteSpace(openId))
                {
                    _logger.WriteError("openid was not found");
                    return new AuthenticationTicket(null, authenticationProperties);
                }

                var userInfoResult = await GetUserInfoResult(accessToken, openId);

                var identity = QQConncetHelper.BuildClaimsIdentity(Options.ClientId, Options.AuthenticationType, accessTokenResult, openIdResult, userInfoResult);

                return new AuthenticationTicket(identity, authenticationProperties);
            }
            catch (Exception ex)
            {
                _logger.WriteError("Authentication failed", ex);
                return new AuthenticationTicket(null, authenticationProperties);
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

        private async Task<JObject> GetUserInfoResult(string accessToken, string openId)
        {
            var response = await _httpClient.GetAsync(BuilUserInfoUrl(accessToken, openId), Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
            return QQConncetHelper.ParseUserInfoResult(text);
        }

        private async Task<JObject> GetOpenIdResult(string accessToken)
        {
            var response = await _httpClient.GetAsync(BuildOpenIdUrl(accessToken), Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
            return QQConncetHelper.ParseOpenIdResult(text);
        }

        private async Task<IReadOnlyDictionary<string, string>> GetAccessTokenResult(string code)
        {
            var response = await _httpClient.GetAsync(BuildAccessTokenUrl(code), Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
            return QQConncetHelper.ParseAccessTokenResult(text);
        }

        private string BuilUserInfoUrl(string accessToken, string openId)
        {
            return QQConncetHelper.BuilUserInfoUrl(
                userInfoEndpoint: Options.UserInformationEndpoint,
                clientId: Options.ClientId,
                openId: openId,
                accessToken: accessToken);
        }

        private string BuildOpenIdUrl(string accessToken)
        {
            return QQConncetHelper.BuildOpenIdUrl(
                    openIdEndpoint: Options.OpenIdEndpoint,
                    accessToken: accessToken);
        }

        private string BuildAccessTokenUrl(string code)
        {
            return QQConncetHelper.BuildAccessTokenUrl(
                    accessTokenEndpoint: Options.TokenEndpoint,
                    clientId: Options.ClientId,
                    clientSecret: Options.ClientSecret,
                    code: code,
                    redirectUri: RedirectUri);
        }

        private string BuildAuthorizationUrl(AuthenticationProperties authenticationProperties)
        {
            var signInParams = authenticationProperties.GetQQConncetSignInParams();
            var scopes = Options.Scopes;
            var displayMode = Options.DisplayMode;
            if (signInParams != null)
            {
                if (signInParams.Scopes != null && signInParams.Scopes.Length > 0)
                {
                    scopes = signInParams.Scopes;
                }

                if (string.IsNullOrWhiteSpace(signInParams.DisplayMode) == false)
                {
                    displayMode = signInParams.DisplayMode;
                }

                authenticationProperties.RemoveQQConncetSignInParams();
            }

            var state = Options.StateDataFormat.Protect(authenticationProperties);

            return QQConncetHelper.BuildAuthorizationUrl(
                    authorizationEndpoint: Options.AuthorizationEndpoint,
                    clientId: Options.ClientId,
                    scopes: scopes,
                    state: state,
                    redirectUri: RedirectUri,
                    displayMode: displayMode);
        }

        private string RedirectUri => Request.Scheme +
                                      Uri.SchemeDelimiter +
                                      Request.Host +
                                      Request.PathBase +
                                      Options.CallbackPath;
    }
}