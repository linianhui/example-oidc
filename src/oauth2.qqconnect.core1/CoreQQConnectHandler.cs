using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http.Authentication;

namespace OAuth2.QQConnect.Core1
{
    internal class CoreQQConnectHandler : OAuthHandler<CoreQQConnectOptions>
    {
        private readonly QQConncetHandler _inner;

        public CoreQQConnectHandler(HttpClient httpClient) : base(httpClient)
        {
            var qqConnectOptions = Options.BuildQQConnectOptions(GetRedirectUrl);
            _inner = new QQConncetHandler(httpClient, qqConnectOptions);
        }

        protected override async Task<AuthenticateResult> HandleRemoteAuthenticateAsync()
        {
            try
            {
                var code = Request.Query["code"].ToString();
                var state = Request.Query["state"].ToString();

                var properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return AuthenticateResult.Fail("The oauth state was missing or invalid.");
                }

                if (ValidateCorrelationId(properties) == false)
                {
                    return AuthenticateResult.Fail("Correlation failed.");
                }

                if (code == null)
                {
                    return AuthenticateResult.Fail("Code was not found.");
                }

                var token = await _inner.GetTokenAsync(
                    code,
                    Context.RequestAborted);

                if (string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    return AuthenticateResult.Fail("OAuth token endpoint failure.");
                }


                var openId = await _inner.GetOpenIdAsync(
                    token.AccessToken,
                    Context.RequestAborted);

                if (string.IsNullOrWhiteSpace(openId.OpenId))
                {
                    return AuthenticateResult.Fail("openid was not found.");
                }


                var user = await _inner.GetUserAsync(
                    token.AccessToken,
                    openId.OpenId,
                    Context.RequestAborted);

                var identity = QQConncetProfile.BuildClaimsIdentity(Options.AuthenticationScheme, token, openId, user);

                var principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, properties, Options.AuthenticationScheme);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var qqConnectProperties = properties.Items.GetQQConnectProperties();
            properties.Items.RemoveQQConnectProperties();

            var state = Options.StateDataFormat.Protect(properties);

            return _inner.BuildAuthorizationUrl(qqConnectProperties, state);
        }

        private string GetRedirectUrl()
        {
            return Request.Scheme
                + "://"
                + Request.Host
                + OriginalPathBase
                + Options.CallbackPath;
        }
    }
}