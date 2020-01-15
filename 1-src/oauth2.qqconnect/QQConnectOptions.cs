using OAuth2.QQConnect.Extensions;
using System;
using System.Collections.Generic;

namespace OAuth2.QQConnect
{
    public sealed class QQConnectOptions
    {
        public string AuthorizationEndpoint { get; } = "https://graph.qq.com/oauth2.0/authorize";

        public string TokenEndpoint { get; } = "https://graph.qq.com/oauth2.0/token";

        public string OpenIdEndpoint { get; } = "https://graph.qq.com/oauth2.0/me";

        public string UserInfoEndpoint { get; } = "https://graph.qq.com/user/get_user_info";

        public string ClientId { get; }

        public string ClientSecret { get; }

        public bool IsMobile { get; }

        public ISet<string> Scopes { get; } = new HashSet<string> { "get_user_info" };

        public Func<string> RedirectUrl { get; set; }

        public QQConnectOptions(string clientId, string clientSecret, bool isMobile, ISet<string> scopes)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            ClientId = clientId;

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }
            ClientSecret = clientSecret;

            IsMobile = isMobile;

            Scopes.AddAll(scopes);
        }

        public string BuildAuthorizationUrl(QQConnectProperties properties, string state)
        {
            var scopes = new HashSet<string>()
                .AddAll(Scopes);

            var isMobile = IsMobile;
            if (properties != null)
            {
                scopes.AddAll(properties.Scopes);
                isMobile = properties.IsMobile;
            }

            var scope = string.Join(",", scopes);

            var authorizationUrl = AuthorizationEndpoint
                                   + "?response_type=code"
                                   + "&client_id=" + Uri.EscapeDataString(ClientId)
                                   + "&redirect_uri=" + Uri.EscapeDataString(RedirectUrl())
                                   + "&state=" + Uri.EscapeDataString(state)
                                   + "&scope=" + Uri.EscapeDataString(scope);

            if (isMobile)
            {
                authorizationUrl += "&display=mobile";
            }

            return authorizationUrl;
        }

        public string BuildAccessTokenUrl(string code)
        {
            return TokenEndpoint
                   + "?grant_type=authorization_code"
                   + "&client_id=" + Uri.EscapeDataString(ClientId)
                   + "&client_secret=" + Uri.EscapeDataString(ClientSecret)
                   + "&redirect_uri=" + Uri.EscapeDataString(RedirectUrl())
                   + "&code=" + Uri.EscapeDataString(code);
        }

        public string BuildOpenIdUrl(string accessToken)
        {
            return OpenIdEndpoint
                   + "?access_token=" + Uri.EscapeDataString(accessToken);
        }

        public string BuildUserInfoUrl(string accessToken, string openId)
        {
            return UserInfoEndpoint
                   + "?access_token=" + Uri.EscapeDataString(openId)
                   + "&oauth_consumer_key=" + Uri.EscapeDataString(ClientId)
                   + "&openid=" + Uri.EscapeDataString(accessToken);
        }
    }
}