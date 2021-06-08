using Newtonsoft.Json.Linq;

namespace OAuth2.QQConnect.Extensions
{
    public static class JObjectExtension
    {
        public static string TryGetValue(this JObject @this, string name)
        {
            if (@this == null)
            {
                return null;
            }

            if (@this.TryGetValue(name, out var value))
            {
                return value.ToString();
            }

            return null;
        }
    }
}
