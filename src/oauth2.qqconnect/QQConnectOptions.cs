using System;
using System.Collections.Generic;

namespace OAuth2.QQConnect.Basic
{
    public sealed class QQConnectOptions
    {
        public string ClientId { get; }

        public string ClientSecret { get; }

        public bool IsMobile { get; }

        public ICollection<string> Scopes { get; }

        public Func<string> RedirectUrl { get; set; }

        public QQConnectOptions(string clientId, string clientSecret, bool isMobile, ICollection<string> scopes)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            ClientId = clientId;
            ClientSecret = clientSecret;
            IsMobile = isMobile;
            Scopes = scopes ?? throw new ArgumentNullException(nameof(scopes));
        }
    }
}