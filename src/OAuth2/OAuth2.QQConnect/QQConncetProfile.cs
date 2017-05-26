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
            this.Idp = FindFirst(QQConnectConstants.IdpClaimType);
            this.AppId = FindFirst(QQConnectConstants.AppIdClaimType);
            this.OpenId = FindFirst(QQConnectConstants.OpenIdClaimType);
            this.NickName = FindFirst(QQConnectConstants.NickNameClaimType);
            this.AvatarUrl = FindFirst(QQConnectConstants.AvatarUrlClaimType);
            this.AccessToken = FindFirst(QQConnectConstants.AccessTokenClaimType);
            this.RefreshToken = FindFirst(QQConnectConstants.RefreshTokenClaimType);
            this.ExpiresIn = int.Parse(FindFirst(QQConnectConstants.ExpiresInClaimType));
        }

        public IEnumerable<Claim> Claims { get; }

        public string Idp { get; }

        public string AppId { get; }

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