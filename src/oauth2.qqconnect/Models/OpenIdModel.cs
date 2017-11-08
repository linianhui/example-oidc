using Newtonsoft.Json.Linq;

namespace OAuth2.QQConnect.Basic.Models
{
    public sealed class OpenIdModel
    {
        private OpenIdModel() { }

        public string ClientId { get; private set; }

        public string OpenId { get; private set; }

        public static OpenIdModel From(string jsonpOpenId)
        {
            //狗日的腾讯！！！jsonp
            //callback( {"client_id":"YOUR_APPID","openid":"YOUR_OPENID"} );
            var json = jsonpOpenId.Substring(8).Trim().Trim('(', ')', ';');
            var openId = JObject.Parse(json);
            return new OpenIdModel
            {
                ClientId = openId.TryGetValue("client_id"),
                OpenId = openId.TryGetValue("openid")
            };
        }
    }
}
