using System;

namespace Client.AuthorizationCodeFlow.UWP.Oidc
{
    /// <summary>
    /// <see cref="http://server.ids3.dev/auth/.well-known/openid-configuration"/>
    /// </summary>
    public class Ids3Options
    {
        public string AuthorizeEndpoint => "http://server.ids3.dev/auth/connect/authorize";
        public string AccessTokenEndpoint { get; set; }
        public string ClientId => "oauth2-authorization-code-flow.uwp";
        public string ClientSecret => "lnh";
        public string RedirectUri => "http://uwp.oauth2-authorization-code-flow.dev/oauth2-callback";

        public string BuildAuthorizeUrl()
        {
            return AuthorizeEndpoint
                      + $"?client_id={ClientId}"
                      + "&scope=openid"
                      + "&response_type=code"
                      + "&response_mode=query"
                      + $"&redirect_uri={RedirectUri}"
                      + $"&state={Guid.NewGuid()}"
                      + $"&nonce={Guid.NewGuid()}";
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
