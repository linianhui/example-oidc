using System;
using System.Collections.Generic;

namespace OAuth2.QQConnect.Extensions
{
    public static class QQConnectPropertiesExtension
    {
        public static void SetQQConnectProperties(
            this IDictionary<string, string> @this,
            QQConnectProperties qqConnectProperties)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (qqConnectProperties == null)
            {
                throw new ArgumentNullException(nameof(qqConnectProperties));
            }

            @this[QQConnectProperties.Key] = qqConnectProperties.ToJson();
        }

        public static QQConnectProperties GetQQConnectProperties(
            this IDictionary<string, string> @this)
        {
            if (@this == null)
            {
                return null;
            }
            if (@this.ContainsKey(QQConnectProperties.Key) == false)
            {
                return null;
            }
            return QQConnectProperties.From(@this[QQConnectProperties.Key]);
        }

        public static void RemoveQQConnectProperties(
            this IDictionary<string, string> @this)
        {
            if (@this?.ContainsKey(QQConnectProperties.Key) == true)
            {
                @this.Remove(QQConnectProperties.Key);
            }
        }
    }
}