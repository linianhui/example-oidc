using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OAuth2.QQConnect
{
    internal static class QQConncetHelper
    {
        public static ClaimsIdentity BuildClaimsIdentity(string clientId, string issuer, IReadOnlyDictionary<string, string> accessTokenResult, JObject openIdResult, JObject userInfoResult)
        {
            var openId = openIdResult.TryGetValue(QQConnectDefaults.OpenIdField);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, openId, ClaimValueTypes.String, issuer),
                new Claim(QQConnectDefaults.IdpClaimType, issuer, ClaimValueTypes.String,issuer),
                new Claim(QQConnectDefaults.ClientIdClaimType, clientId, ClaimValueTypes.String,issuer),
                new Claim(QQConnectDefaults.OpenIdClaimType, openId, ClaimValueTypes.String, issuer),
                new Claim(QQConnectDefaults.AccessTokenClaimType, accessTokenResult.TryGetValue(QQConnectDefaults.AccessTokenField), ClaimValueTypes.String,issuer),
                new Claim(QQConnectDefaults.RefreshTokenClaimType,accessTokenResult.TryGetValue(QQConnectDefaults.RefreshTokenField), ClaimValueTypes.String,issuer),
                new Claim(QQConnectDefaults.ExpiresInClaimType, accessTokenResult.TryGetValue(QQConnectDefaults.ExpiresInField), ClaimValueTypes.String,issuer),
                new Claim(QQConnectDefaults.NickNameClaimType, userInfoResult.TryGetValue(QQConnectDefaults.NickNameField),ClaimValueTypes.String, issuer),
                new Claim(QQConnectDefaults.AvatarUrlClaimType, userInfoResult.TryGetValue(QQConnectDefaults.AvatarUrlField),ClaimValueTypes.String, issuer),
            };

            return new ClaimsIdentity(claims, issuer, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public static JObject ParseUserInfoResult(string userInfoResultJson)
        {
            //狗日的腾讯！！！json
            return JObject.Parse(userInfoResultJson);
        }

        public static JObject ParseOpenIdResult(string openIdResultJsonp)
        {
            //狗日的腾讯！！！jsonp
            //callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} );
            var json = openIdResultJsonp.Substring(8).Trim().Trim('(', ')', ';');
            return JObject.Parse(json);
        }

        public static IReadOnlyDictionary<string, string> ParseAccessTokenResult(string accessTokenResultFormUrlEncoded)
        {
            //狗日的腾讯！！！Content-Type是text/html,格式却是form-urlencoded
            //access_token=YOUR_ACCESS_TOKEN&expires_in=3600
            var keyValues = new Dictionary<string, string>();
            foreach (var param in accessTokenResultFormUrlEncoded.Split('&'))
            {
                var keyValue = param.Split('=');
                keyValues.Add(keyValue[0], keyValue[1]);
            }
            return keyValues;
        }

        public static string BuilUserInfoUrl(string userInfoEndpoint, string clientId, string openId, string accessToken)
        {
            return userInfoEndpoint
                    + $"?access_token={Uri.EscapeDataString(accessToken)}"
                    + $"&oauth_consumer_key={Uri.EscapeDataString(clientId)}"
                    + $"&openid={Uri.EscapeDataString(openId)}";
        }

        public static string BuildOpenIdUrl(string openIdEndpoint, string accessToken)
        {
            return $"{openIdEndpoint}?access_token={Uri.EscapeDataString(accessToken)}";
        }

        public static string BuildAccessTokenUrl(string accessTokenEndpoint, string clientId, string clientSecret, string code, string redirectUri)
        {
            return accessTokenEndpoint
                    + "?grant_type=authorization_code"
                    + $"&client_id={Uri.EscapeDataString(clientId)}"
                    + $"&client_secret={Uri.EscapeDataString(clientSecret)}"
                    + $"&redirect_uri={Uri.EscapeDataString(redirectUri)}"
                    + $"&code={Uri.EscapeDataString(code)}";
        }

        public static string BuildAuthorizationUrl(string authorizationEndpoint, string clientId, string[] scopes, string state, string redirectUri, string displayMode)
        {
            var scope = String.Join(",", scopes);

            var authorizationUrl = authorizationEndpoint
                                 + "?response_type=code"
                                 + $"&client_id={Uri.EscapeDataString(clientId)}"
                                 + $"&redirect_uri={Uri.EscapeDataString(redirectUri)}"
                                 + $"&state={Uri.EscapeDataString(state)}"
                                 + $"&scope={Uri.EscapeDataString(scope)}";

            if (String.IsNullOrWhiteSpace(displayMode) == false)
            {
                authorizationUrl += $"&display={Uri.EscapeDataString(displayMode.Trim())}";
            }

            return authorizationUrl;
        }

        public static void SetQQConncetSignInParams(this IDictionary<string, string> dictionary, QQConnectSignInParams qqConnectSignInParams)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (qqConnectSignInParams == null)
            {
                throw new ArgumentNullException(nameof(qqConnectSignInParams));
            }

            dictionary[QQConnectSignInParams.Key] = qqConnectSignInParams.ToString();
        }

        public static QQConnectSignInParams GetQQConncetSignInParams(this IDictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            if (dictionary.ContainsKey(QQConnectSignInParams.Key) == false)
            {
                return null;
            }
            return QQConnectSignInParams.From(dictionary[QQConnectSignInParams.Key]);
        }

        public static void RemoveQQConncetSignInParams(this IDictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            if (dictionary.ContainsKey(QQConnectSignInParams.Key) == true)
            {
                dictionary.Remove(QQConnectSignInParams.Key);
            }
        }
    }
}