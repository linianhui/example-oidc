using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2.QQConnect
{
    internal class QQConnectAuthenticationHandler : AuthenticationHandler<QQConnectAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public QQConnectAuthenticationHandler(ILogger logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
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

                var accessTokenResult = await GetAccessTokenResult(code);
                var accessToken = accessTokenResult["access_token"];
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    _logger.WriteError("access_token was not found");
                    return new AuthenticationTicket(null, properties);
                }

                var openIdResult = await GetOpenIdResult(accessToken);
                var openId = openIdResult.Value<string>("openid");
                if (string.IsNullOrWhiteSpace(openId))
                {
                    _logger.WriteError("openid was not found");
                    return new AuthenticationTicket(null, properties);
                }

                var userInfoResult = await GetUserInfoResult(accessToken, openId);
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

        private async Task<JObject> GetUserInfoResult(string accessToken, string openId)
        {
            var response = await _httpClient.GetAsync(BuilUserInfoUrl(accessToken, openId), Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JObject.Parse(json);
        }

        private async Task<JObject> GetOpenIdResult(string accessToken)
        {
            var response = await _httpClient.GetAsync(BuildOpenIdUrl(accessToken), Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
            var json = text.Substring(8).Trim().Trim('(', ')', ';');
            return JObject.Parse(json);
        }

        private async Task<IReadOnlyDictionary<string, string>> GetAccessTokenResult(string code)
        {
            var response = await _httpClient.GetAsync(BuildAccessTokenUrl(code), Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
            var keyValues = new Dictionary<string, string>();
            foreach (var param in text.Split('&'))
            {
                var keyValue = param.Split('=');
                keyValues.Add(keyValue[0], keyValue[1]);
            }
            return keyValues;
        }

        private string BuilUserInfoUrl(string accessToken, string openId)
        {
            var userInfoUrlBuilder = new StringBuilder(Options.UserInfoEndpoint);

            userInfoUrlBuilder.Append($"?access_token={Uri.EscapeDataString(accessToken)}");
            userInfoUrlBuilder.Append($"&oauth_consumer_key={Uri.EscapeDataString(Options.AppId)}");
            userInfoUrlBuilder.Append($"&openid={Uri.EscapeDataString(openId)}");

            return userInfoUrlBuilder.ToString();
        }

        private string BuildOpenIdUrl(string accessToken)
        {
            var openIdUrlBuilder = new StringBuilder(Options.OpenIdEndpoint);

            openIdUrlBuilder.Append($"?access_token={Uri.EscapeDataString(accessToken)}");

            return openIdUrlBuilder.ToString();
        }

        private string BuildAccessTokenUrl(string code)
        {
            var accessTokenUrlBuilder = new StringBuilder(Options.AccessTokenEndpoint);

            accessTokenUrlBuilder.Append("?grant_type=authorization_code");
            accessTokenUrlBuilder.Append($"&client_id={Uri.EscapeDataString(Options.AppId)}");
            accessTokenUrlBuilder.Append($"&client_secret={Uri.EscapeDataString(Options.AppSecret)}");
            accessTokenUrlBuilder.Append($"&redirect_uri={Uri.EscapeDataString(RedirectUri)}");
            accessTokenUrlBuilder.Append($"&code={Uri.EscapeDataString(code)}");

            return accessTokenUrlBuilder.ToString();
        }

        private string BuildAuthorizationUrl(AuthenticationProperties authenticationProperties)
        {
            var state = Options.StateDataFormat.Protect(authenticationProperties);
            var scope = string.Join(",", Options.Scopes);

            var authorizationUrlBuilder = new StringBuilder(Options.AuthorizationEndpoint);

            authorizationUrlBuilder.Append("?response_type=code");
            authorizationUrlBuilder.Append($"&client_id={Uri.EscapeDataString(Options.AppId)}");
            authorizationUrlBuilder.Append($"&redirect_uri={Uri.EscapeDataString(RedirectUri)}");
            authorizationUrlBuilder.Append($"&state={Uri.EscapeDataString(state)}");
            authorizationUrlBuilder.Append($"&scope={Uri.EscapeDataString(scope)}");

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