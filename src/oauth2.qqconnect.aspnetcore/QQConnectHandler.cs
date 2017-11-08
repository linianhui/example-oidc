using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OAuth2.QQConnect.Basic;
using BasicQQConnectHandler = OAuth2.QQConnect.Basic.QQConnectHandler;

namespace OAuth2.QQConnect.AspNetCore
{
    internal class QQConnectHandler : OAuthHandler<QQConnectOptions>
    {
        private BasicQQConnectHandler _innerHandler;

        private BasicQQConnectHandler InnerHandler
        {
            get
            {
                if (_innerHandler == null)
                {
                    var basicQQConnectOptions = Options.BuildQQConnectOptions(GetRedirectUrl);
                    _innerHandler = new BasicQQConnectHandler(Backchannel, basicQQConnectOptions);
                }
                return _innerHandler;
            }
        }

        public QQConnectHandler(
            IOptionsMonitor<QQConnectOptions> options,
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

                var properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return HandleRequestResult.Fail("The oauth state was missing or invalid.");
                }

                if (ValidateCorrelationId(properties) == false)
                {
                    return HandleRequestResult.Fail("Correlation failed.");
                }

                if (code == null)
                {
                    return HandleRequestResult.Fail("Code was not found.");
                }

                var token = await InnerHandler.GetTokenAsync(
                    code,
                    Context.RequestAborted);

                if (string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    return HandleRequestResult.Fail("OAuth token endpoint failure.");
                }


                var openId = await InnerHandler.GetOpenIdAsync(
                    token.AccessToken,
                    Context.RequestAborted);

                if (string.IsNullOrWhiteSpace(openId.OpenId))
                {
                    return HandleRequestResult.Fail("openid was not found.");
                }


                var user = await InnerHandler.GetUserAsync(
                    token.AccessToken,
                    openId.OpenId,
                    Context.RequestAborted);

                var identity = QQConnectProfile.BuildClaimsIdentity(Scheme.Name, token, openId, user);

                var principal = new ClaimsPrincipal(identity);

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

            return InnerHandler.BuildAuthorizationUrl(qqConnectProperties, state);
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