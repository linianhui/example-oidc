using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuth2.QQConnect
{
    internal class QQConnectHandler : OAuthHandler<QQConnectOptions>
    {
        public QQConnectHandler(HttpClient httpClient)
            : base(httpClient)
        {
        }

        protected override async Task<AuthenticateResult> HandleRemoteAuthenticateAsync()
        {
            try
            {
                var code = Request.Query["code"][0];
                var state = Request.Query["state"][0];

                var authenticationProperties = Options.StateDataFormat.Unprotect(state);
                if (authenticationProperties == null)
                {
                    return null;
                }

                if (ValidateCorrelationId(authenticationProperties) == false)
                {
                    return AuthenticateResult.Fail("");
                }

                if (code == null)
                {
                    return AuthenticateResult.Fail("code is null");
                }

                var accessTokenResult = await GetAccessTokenResult(code);
                var accessToken = accessTokenResult[QQConnectDefaults.AccessTokenField];
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    return AuthenticateResult.Fail("access_token was not found");
                }

                var openIdResult = await GetOpenIdResult(accessToken);
                var openId = openIdResult.TryGetValue(QQConnectDefaults.OpenIdField);
                if (string.IsNullOrWhiteSpace(openId))
                {
                    return AuthenticateResult.Fail("openid was not found");
                }

                var userInfoResult = await GetUserInfoResult(accessToken, openId);

                var identity = QQConncetHelper.BuildClaimsIdentity(Options.ClientId, Options.AuthenticationScheme, accessTokenResult, openIdResult, userInfoResult);

                return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), authenticationProperties, Options.AuthenticationScheme));
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var signInParams = properties.GetQQConncetSignInParams();
            var scopes = Options.Scope.ToArray();
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

                properties.RemoveQQConncetSignInParams();
            }

            var state = Options.StateDataFormat.Protect(properties);

            return QQConncetHelper.BuildAuthorizationUrl(
                authorizationEndpoint: Options.AuthorizationEndpoint,
                clientId: Options.ClientId,
                scopes: scopes,
                state: state,
                redirectUri: redirectUri,
                displayMode: displayMode);
        }

        private async Task<JObject> GetUserInfoResult(string accessToken, string openId)
        {
            var response = await base.Backchannel.GetAsync(BuilUserInfoUrl(accessToken, openId), Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
            return QQConncetHelper.ParseUserInfoResult(text);
        }

        private async Task<JObject> GetOpenIdResult(string accessToken)
        {
            var response = await base.Backchannel.GetAsync(BuildOpenIdUrl(accessToken), Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
            return QQConncetHelper.ParseOpenIdResult(text);
        }

        private async Task<IReadOnlyDictionary<string, string>> GetAccessTokenResult(string code)
        {
            var response = await base.Backchannel.GetAsync(BuildAccessTokenUrl(code), Context.RequestAborted);
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
                redirectUri: BuildRedirectUri(Options.CallbackPath));
        }
    }
}