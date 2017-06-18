using Newtonsoft.Json;

namespace OAuth2.QQConnect
{
    public class QQConnectSignInParams
    {
        public static readonly string Key = "qq-connect.signin-params";

        public string[] Scopes { get; set; }

        public string DisplayMode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        public static QQConnectSignInParams From(string json)
        {
            return JsonConvert.DeserializeObject<QQConnectSignInParams>(json);
        }
    }
}