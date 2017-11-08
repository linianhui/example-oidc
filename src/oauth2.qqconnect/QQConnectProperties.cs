using Newtonsoft.Json;

namespace OAuth2.QQConnect.Basic
{
    public sealed class QQConnectProperties
    {
        internal static readonly string Key = "qq-connect.props";

        public string[] Scopes { get; set; }

        public bool IsMobile { get; set; }

        internal string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None);
        }

        internal static QQConnectProperties From(string json)
        {
            return JsonConvert.DeserializeObject<QQConnectProperties>(json);
        }
    }
}