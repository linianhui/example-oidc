using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using OAuth2.QQConnect.Basic.Models;

namespace OAuth2.QQConnect.Basic
{
    /// <summary>
    /// <see cref="http://wiki.connect.qq.com/%e5%bc%80%e5%8f%91%e6%94%bb%e7%95%a5_server-side"/>
    /// </summary>
    public class QQConnectHandler
    {
        private const string AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";
        private const string TokenEndpoint = "https://graph.qq.com/oauth2.0/token";
        private const string OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";
        private const string UserInfoEndpoint = "https://graph.qq.com/user/get_user_info";

        private readonly HttpClient _httpClient;
        private readonly QQConnectOptions _options;

        public QQConnectHandler(
            HttpClient httpClient,
            QQConnectOptions options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public string BuildAuthorizationUrl(QQConnectProperties qqConnectProperties, string state)
        {
            var scopes = _options.Scopes;
            var isMobile = _options.IsMobile;
            if (qqConnectProperties != null)
            {
                if (qqConnectProperties.Scopes != null)
                {
                    scopes = qqConnectProperties.Scopes;
                }
                isMobile = qqConnectProperties.IsMobile;
            }
            var scope = string.Join(",", scopes);

            var authorizationUrl = AuthorizationEndpoint
                                 + "?response_type=code"
                                 + "&client_id=" + Uri.EscapeDataString(_options.ClientId)
                                 + "&redirect_uri=" + Uri.EscapeDataString(_options.RedirectUrl())
                                 + "&state=" + Uri.EscapeDataString(state)
                                 + "&scope=" + Uri.EscapeDataString(scope);

            if (isMobile)
            {
                authorizationUrl += "&display=mobile";
            }

            return authorizationUrl;
        }

        public async Task<UserModel> GetUserAsync(string accessToken, string openId, CancellationToken cancellationToken)
        {
            var userInfoUrl = UserInfoEndpoint
                              + "?access_token=" + Uri.EscapeDataString(openId)
                              + "&oauth_consumer_key=" + Uri.EscapeDataString(_options.ClientId)
                              + "&openid=" + Uri.EscapeDataString(accessToken);

            var response = await GetStringResponseAsync(userInfoUrl, cancellationToken);

            return UserModel.From(response);
        }

        public async Task<OpenIdModel> GetOpenIdAsync(string accessToken, CancellationToken cancellationToken)
        {
            var openIdUrl = OpenIdEndpoint + "?access_token=" + Uri.EscapeDataString(accessToken);

            var response = await GetStringResponseAsync(openIdUrl, cancellationToken);

            return OpenIdModel.From(response);
        }

        public async Task<TokenModel> GetTokenAsync(string code, CancellationToken cancellationToken)
        {
            var accessTokenUrl = TokenEndpoint
                                 + "?grant_type=authorization_code"
                                 + "&client_id=" + Uri.EscapeDataString(_options.ClientId)
                                 + "&client_secret=" + Uri.EscapeDataString(_options.ClientSecret)
                                 + "&redirect_uri=" + Uri.EscapeDataString(_options.RedirectUrl())
                                 + "&code=" + Uri.EscapeDataString(code);

            var response = await GetStringResponseAsync(accessTokenUrl, cancellationToken);

            return TokenModel.From(response);
        }


        private async Task<string> GetStringResponseAsync(string url, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}