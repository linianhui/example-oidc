using OAuth2.QQConnect.Response;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OAuth2.QQConnect
{
    /// <summary>
    /// <see cref="http://wiki.connect.qq.com/%e5%bc%80%e5%8f%91%e6%94%bb%e7%95%a5_server-side"/>
    /// </summary>
    public class QQConnectClient
    {
        private readonly HttpClient _httpClient;
        private readonly QQConnectOptions _options;

        public QQConnectClient(HttpClient httpClient, QQConnectOptions options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public string BuildAuthorizationUrl(QQConnectProperties properties, string state)
        {
            return _options.BuildAuthorizationUrl(properties, state);
        }

        public async Task<TokenResponse> GetTokenAsync(string code, CancellationToken cancellationToken)
        {
            var accessTokenUrl = _options.BuildAccessTokenUrl(code);

            var response = await GetStringAsync(accessTokenUrl, cancellationToken);

            return TokenResponse.From(response);
        }

        public async Task<OpenIdResponse> GetOpenIdAsync(string accessToken, CancellationToken cancellationToken)
        {
            var openIdUrl = _options.BuildOpenIdUrl(accessToken);

            var response = await GetStringAsync(openIdUrl, cancellationToken);

            return OpenIdResponse.From(response);
        }

        public async Task<UserResponse> GetUserAsync(string accessToken, string openId, CancellationToken cancellationToken)
        {
            var userInfoUrl = _options.BuildUserInfoUrl(accessToken, openId);

            var response = await GetStringAsync(userInfoUrl, cancellationToken);

            return UserResponse.From(response);
        }

        private async Task<string> GetStringAsync(string url, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}