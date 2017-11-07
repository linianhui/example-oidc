using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using OAuth2.QQConnect.Models;

namespace OAuth2.QQConnect
{
    public sealed class QQConncetProfile
    {
        public QQConncetProfile(IEnumerable<Claim> claims)
        {
            Claims = claims ?? throw new ArgumentNullException(nameof(claims));
            Issuer = FindFirst(ClaimTypes.Issuer);
            ClientId = FindFirst(ClaimTypes.ClientId);
            OpenId = FindFirst(ClaimTypes.OpenId);
            NickName = FindFirst(ClaimTypes.NickName);
            Avatar = FindFirst(ClaimTypes.Avatar);
            AccessToken = FindFirst(ClaimTypes.AccessToken);
            RefreshToken = FindFirst(ClaimTypes.RefreshToken);
            ExpiresIn = int.Parse(FindFirst(ClaimTypes.ExpiresIn));
        }

        public IEnumerable<Claim> Claims { get; }

        public string Issuer { get; }

        public string ClientId { get; }

        public string OpenId { get; }

        public string NickName { get; }

        public string Avatar { get; }

        public string AccessToken { get; }

        public string RefreshToken { get; }

        public int ExpiresIn { get; }

        private string FindFirst(string claimType)
        {
            return Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
        }

        public static ClaimsIdentity BuildClaimsIdentity(string issuer, TokenModel token, OpenIdModel openId, UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(System.Security.Claims.ClaimTypes.NameIdentifier, openId.OpenId, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.Issuer, issuer, ClaimValueTypes.String,issuer),
                new Claim(ClaimTypes.ClientId, openId.ClientId, ClaimValueTypes.String,issuer),
                new Claim(ClaimTypes.OpenId, openId.OpenId, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.AccessToken, token.AccessToken, ClaimValueTypes.String,issuer),
                new Claim(ClaimTypes.RefreshToken,token.RefreshToken, ClaimValueTypes.String,issuer),
                new Claim(ClaimTypes.ExpiresIn, token.ExpiresIn, ClaimValueTypes.String,issuer),
                new Claim(ClaimTypes.NickName, user.NickName,ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.Avatar,user.Avatar,ClaimValueTypes.String, issuer),
            };

            return new ClaimsIdentity(
                claims,
                issuer,
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        }

        public static class ClaimTypes
        {
            public static readonly string Prefix = "qq.";

            public static readonly string Issuer = Prefix + "issuer";

            public static readonly string ClientId = Prefix + "client_id";

            public static readonly string OpenId = Prefix + "openid";

            public static readonly string AccessToken = Prefix + "access_token";

            public static readonly string RefreshToken = Prefix + "refresh_token";

            public static readonly string ExpiresIn = Prefix + "expires_in";

            public static readonly string NickName = Prefix + "nickname";

            public static readonly string Avatar = Prefix + "avatar";
        }
    }
}