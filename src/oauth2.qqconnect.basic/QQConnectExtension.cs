using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace OAuth2.QQConnect.Basic
{
    public static class QQConnectExtension
    {
        internal static string TryGetValue(this JObject json, string name)
        {
            if (json == null)
            {
                return string.Empty;
            }

            if (json.TryGetValue(name, out var value))
            {
                return value.ToString();
            }

            return string.Empty;
        }

        internal static string TryGetValue(this IReadOnlyDictionary<string, string> dictionary, string key)
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

        public static void SetQQConnectProperties(
            this IDictionary<string, string> dictionary,
            QQConnectProperties qqConnectProperties)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (qqConnectProperties == null)
            {
                throw new ArgumentNullException(nameof(qqConnectProperties));
            }

            dictionary[QQConnectProperties.Key] = qqConnectProperties.ToJson();
        }

        public static QQConnectProperties GetQQConnectProperties(
            this IDictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            if (dictionary.ContainsKey(QQConnectProperties.Key) == false)
            {
                return null;
            }
            return QQConnectProperties.From(dictionary[QQConnectProperties.Key]);
        }

        public static void RemoveQQConnectProperties(
            this IDictionary<string, string> dictionary)
        {
            if (dictionary?.ContainsKey(QQConnectProperties.Key) == true)
            {
                dictionary.Remove(QQConnectProperties.Key);
            }
        }
    }
}