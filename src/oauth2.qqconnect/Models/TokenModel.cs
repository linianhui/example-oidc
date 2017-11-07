using System.Collections.Generic;

namespace OAuth2.QQConnect.Models
{
    public sealed class TokenModel
    {
        private TokenModel() { }

        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }

        public string ExpiresIn { get; private set; }


        public static TokenModel From(string urlEncodedToken)
        {
            //狗日的腾讯！！！Content-Type是text/html,格式却是form-urlencoded
            //access_token=YOUR_ACCESS_TOKEN&expires_in=3600
            var keyValues = new Dictionary<string, string>();
            foreach (var param in urlEncodedToken.Split('&'))
            {
                var keyValue = param.Split('=');
                keyValues.Add(keyValue[0], keyValue[1]);
            }
            return new TokenModel
            {
                AccessToken = keyValues.TryGetValue("access_token"),
                RefreshToken = keyValues.TryGetValue("refresh_token"),
                ExpiresIn = keyValues.TryGetValue("expires_in")
            };
        }
    }
}
