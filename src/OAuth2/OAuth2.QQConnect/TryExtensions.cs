using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OAuth2.QQConnect
{
    internal static class TryExtensions
    {
        public static string TryGetValue(this JObject json, string name)
        {
            if (json == null)
            {
                return string.Empty;
            }

            JToken jtoken;
            if (json.TryGetValue(name, out jtoken))
            {
                return jtoken.ToString();
            }

            return string.Empty;
        }

        public static string TryGetValue(this IReadOnlyDictionary<string, string> dictionary, string key)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return string.Empty;
        }
    }
}