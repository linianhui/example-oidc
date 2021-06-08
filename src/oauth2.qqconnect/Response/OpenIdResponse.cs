using Newtonsoft.Json.Linq;
using OAuth2.QQConnect.Extensions;

namespace OAuth2.QQConnect.Response
{
    public sealed class OpenIdResponse
    {
        private OpenIdResponse()
        {
        }

        public string ClientId { get; private set; }

        public string OpenId { get; private set; }

        public static OpenIdResponse From(string openIdJsonp)
        {
            //狗日的腾讯！！！jsonp
            //callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} );
            var json = openIdJsonp.Substring(8).Trim().Trim('(', ')', ';');
            var openId = JObject.Parse(json);
            return new OpenIdResponse
            {
                ClientId = openId.TryGetValue("client_id"),
                OpenId = openId.TryGetValue("openid")
            };
        }
    }
}
