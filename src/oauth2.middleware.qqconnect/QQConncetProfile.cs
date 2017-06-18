using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OAuth2.QQConnect
{
    public class QQConncetProfile
    {
        public QQConncetProfile(IEnumerable<Claim> claims)
        {
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            this.Claims = claims;
            this.Idp = FindFirst(QQConnectDefaults.IdpClaimType);
            this.ClientId = FindFirst(QQConnectDefaults.ClientIdClaimType);
            this.OpenId = FindFirst(QQConnectDefaults.OpenIdClaimType);
            this.NickName = FindFirst(QQConnectDefaults.NickNameClaimType);
            this.AvatarUrl = FindFirst(QQConnectDefaults.AvatarUrlClaimType);
            this.AccessToken = FindFirst(QQConnectDefaults.AccessTokenClaimType);
            this.RefreshToken = FindFirst(QQConnectDefaults.RefreshTokenClaimType);
            this.ExpiresIn = int.Parse(FindFirst(QQConnectDefaults.ExpiresInClaimType));
        }

        public IEnumerable<Claim> Claims { get; }

        public string Idp { get; }

        public string ClientId { get; }

        public string OpenId { get; }

        public string NickName { get; }

        public string AvatarUrl { get; }

        public string AccessToken { get; }

        public string RefreshToken { get; }

        public int ExpiresIn { get; }

        private string FindFirst(string claimType)
        {
            return Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
        }
    }
}