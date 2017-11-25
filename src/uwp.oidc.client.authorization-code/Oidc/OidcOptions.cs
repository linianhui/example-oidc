using System;
using System.Collections.Generic;

namespace ClientUwp.Oidc
{
    /// <summary>
    /// <see cref="http://oidc-server.dev/.well-known/openid-configuration"/>
    /// </summary>
    public class OidcOptions
    {
        public string AuthorizeEndpoint => "http://oidc-server.dev/connect/authorize";
        public string TokenEndpoint => "http://oidc-server.dev/connect/token";
        public string ClientId => "oidc-authorization-code-client";
        public string ClientSecret => "lnh";
        public string RedirectUri => "http://oidc-client-authorization-code.dev/oidc/login-callback";

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
