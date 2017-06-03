using System;
using System.Collections.Generic;

namespace Client.AuthorizationCodeFlow.UWP.Oidc
{
    /// <summary>
    /// <see cref="http://server.ids3.dev/auth/.well-known/openid-configuration"/>
    /// </summary>
    public class OidcOptions
    {
        public string AuthorizeEndpoint => "http://server.ids3.dev/auth/connect/authorize";
        public string TokenEndpoint => "http://server.ids3.dev/auth/connect/token";
        public string ClientId => "oauth2-authorization-code-flow.uwp";
        public string ClientSecret => "lnh";
        public string RedirectUri => "http://uwp.oauth2-authorization-code-flow.dev/oauth2-callback";

        public string BuildAuthorizeUrl(string idp = null)
        {
            var authorizeUrl = AuthorizeEndpoint
                      + $"?client_id={ClientId}"
                      + "&scope=openid"
                      + "&response_type=code"
                      + "&response_mode=query"
                      + $"&redirect_uri={RedirectUri}"
                      + $"&state={Guid.NewGuid()}"
                      + $"&nonce={Guid.NewGuid()}";
            if (idp != null)
            {
                authorizeUrl += $"&acr_values=idp:{idp}";
            }
            return authorizeUrl;
        }

        public IReadOnlyDictionary<string, string> BuildTokenParams(string code)
        {
            return new Dictionary<string, string>
            {
                ["client_id"] = ClientId,
                ["client_secret"] = ClientSecret,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = RedirectUri
            };
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
