using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WPFClient.Oidc
{
    /// <summary>
    /// <see cref="http://oidc-server.test/.well-known/openid-configuration"/>
    /// </summary>
    public class OidcClient
    {
        private static readonly HttpClient Http = new HttpClient();
        public static readonly OidcOptions Options = new OidcOptions();

        public string BuildAuthorizeUrl(string idp = null)
        {
            var authorizeUrl = Options.AuthorizeEndpoint
                               + $"?client_id={Options.ClientId}"
                               + "&scope=openid"
                               + "&response_type=code"
                               + "&response_mode=query"
                               + $"&redirect_uri={Options.RedirectUri}"
                               + $"&state={Guid.NewGuid()}"
                               + $"&nonce={Guid.NewGuid()}";
            if (idp != null)
            {
                authorizeUrl += $"&acr_values=idp:{idp}";
            }
            return authorizeUrl;
        }

        public async Task<string> GetTokenAsync(string code)
        {
            var tokenParams = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = Options.ClientId,
                ["client_secret"] = Options.ClientSecret,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = Options.RedirectUri
            });

            var tokenReponse = await Http.PostAsync(Options.TokenEndpoint, tokenParams);
            return await tokenReponse.Content.ReadAsStringAsync();
        }

        public static string GetCode(string querySring)
        {
            querySring = querySring.TrimStart('?');
            var namevalues = querySring.Split('&');
            foreach (var nameValue in namevalues)
            {
                if (nameValue.StartsWith("code="))
                {
                    return nameValue.Substring(5);
                }
            }
            return string.Empty;
        }

    }
}
