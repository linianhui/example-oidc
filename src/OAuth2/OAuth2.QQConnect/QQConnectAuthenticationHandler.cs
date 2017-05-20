using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2.QQConnect
{
    /// <summary>
    /// <see cref="http://wiki.connect.qq.com/%e5%bc%80%e5%8f%91%e6%94%bb%e7%95%a5_server-side"/>
    /// </summary>
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
                    _logger.WriteWarning("Invalid return state, unable to redirect.");
                    Response.StatusCode = 500;
                }
                return true;
            }
            return false;
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
                var accessToken = accessTokenResult["access_token"];
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    _logger.WriteError("access_token was not found");
                    return new AuthenticationTicket(null, authenticationProperties);
                }

                var openIdResult = await GetOpenIdResult(accessToken);
                var openId = GetJsonValue(openIdResult, "openid");
                if (string.IsNullOrWhiteSpace(openId))
                {
                    _logger.WriteError("openid was not found");
                    return new AuthenticationTicket(null, authenticationProperties);
                }

                var userInfoResult = await GetUserInfoResult(accessToken, openId);

                var identity = new ClaimsIdentity(Options.AuthenticationType, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, openId));
                identity.AddClaim(new Claim("nickname", GetJsonValue(userInfoResult, "nickname")));
                identity.AddClaim(new Claim("avatar", GetJsonValue(userInfoResult, "figureurl_qq_1")));

                return new AuthenticationTicket(identity, authenticationProperties);
            }
            catch (Exception ex)
            {
                _logger.WriteError("Authentication failed", ex);
                return new AuthenticationTicket(null, authenticationProperties);
            }
        }

        private static string GetJsonValue(JObject json, string name)
        {
            JToken value;
            json.TryGetValue(name, out value);
            return value?.ToString() ?? string.Empty;
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
            //狗日的腾讯！！！json
            var json = await response.Content.ReadAsStringAsync();
            return JObject.Parse(json);
        }

        private async Task<JObject> GetOpenIdResult(string accessToken)
        {
            var response = await _httpClient.GetAsync(BuildOpenIdUrl(accessToken), Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            //狗日的腾讯！！！jsonp
            //callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} );
            var jsonp = await response.Content.ReadAsStringAsync();
            var json = jsonp.Substring(8).Trim().Trim('(', ')', ';');
            return JObject.Parse(json);
        }

        private async Task<IReadOnlyDictionary<string, string>> GetAccessTokenResult(string code)
        {
            var response = await _httpClient.GetAsync(BuildAccessTokenUrl(code), Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            //狗日的腾讯！！！Content-Type是text/html,格式却是form-urlencoded
            //access_token=YOUR_ACCESS_TOKEN&expires_in=3600
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