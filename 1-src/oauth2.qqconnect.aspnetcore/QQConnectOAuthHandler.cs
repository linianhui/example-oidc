using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OAuth2.QQConnect.Extensions;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OAuth2.QQConnect.AspNetCore
{
    internal class QQConnectOAuthHandler : OAuthHandler<QQConnectOAuthOptions>
    {
        private QQConnectClient _innerClient;

        private QQConnectClient InnerClient
        {
            get
            {
                if (_innerClient == null)
                {
                    var qqConnectOptions = Options.BuildQQConnectOptions(GetRedirectUrl);
                    _innerClient = new QQConnectClient(Backchannel, qqConnectOptions);
                }
                return _innerClient;
            }
        }

        public QQConnectOAuthHandler(
            IOptionsMonitor<QQConnectOAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            try
            {
                var code = Request.Query["code"].ToString();
                var state = Request.Query["state"].ToString();

                if (code == null)
                {
                    return HandleRequestResult.Fail("Code was not found.");
                }

                var properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return HandleRequestResult.Fail("The oauth state was missing or invalid.");
                }

                if (ValidateCorrelationId(properties) == false)
                {
                    return HandleRequestResult.Fail("Correlation failed.");
                }

                var token = await InnerClient.GetTokenAsync(
                    code,
                    Context.RequestAborted);

                if (string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    return HandleRequestResult.Fail("OAuth token endpoint failure.");
                }

                var openId = await InnerClient.GetOpenIdAsync(
                    token.AccessToken,
                    Context.RequestAborted);

                if (string.IsNullOrWhiteSpace(openId.OpenId))
                {
                    return HandleRequestResult.Fail("openid was not found.");
                }

                var user = await InnerClient.GetUserAsync(
                    token.AccessToken,
                    openId.OpenId,
                    Context.RequestAborted);

                var qqConnectProfile = QQConnectProfile.From(Scheme.Name, token, openId, user);

                var principal = qqConnectProfile.BuildClaimsPrincipal();

                var ticket = new AuthenticationTicket(principal, properties, Scheme.Name);

                return HandleRequestResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return HandleRequestResult.Fail(ex);
            }
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var qqConnectProperties = properties.Items.GetQQConnectProperties();
            properties.Items.RemoveQQConnectProperties();

            var state = Options.StateDataFormat.Protect(properties);

            return InnerClient.BuildAuthorizationUrl(qqConnectProperties, state);
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