using OAuth2.QQConnect.Extensions;
using OAuth2.QQConnect.Response;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OAuth2.QQConnect
{
    public sealed partial class QQConnectProfile
    {
        public static QQConnectProfile From(ICollection<Claim> claims)
        {
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            return new QQConnectProfile
            {
                Claims = claims,
                Issuer = claims.FindFirstValue(ClaimTypes.Issuer),
                ClientId = claims.FindFirstValue(ClaimTypes.ClientId),
                OpenId = claims.FindFirstValue(ClaimTypes.OpenId),
                NickName = claims.FindFirstValue(ClaimTypes.NickName),
                Avatar = claims.FindFirstValue(ClaimTypes.Avatar),
                AccessToken = claims.FindFirstValue(ClaimTypes.AccessToken),
                RefreshToken = claims.FindFirstValue(ClaimTypes.RefreshToken),
                ExpiresIn = int.Parse(claims.FindFirstValue(ClaimTypes.ExpiresIn))
            };
        }

        public static QQConnectProfile From(string issuer, TokenResponse token, OpenIdResponse openId, UserResponse user)
        {
            if (issuer == null)
            {
                throw new ArgumentNullException(nameof(issuer));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (openId == null)
            {
                throw new ArgumentNullException(nameof(openId));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return new QQConnectProfile
            {
                Issuer = issuer,
                ClientId = openId.ClientId,
                OpenId = openId.OpenId,
                NickName = user.NickName,
                Avatar = user.Avatar,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpiresIn = int.Parse(token.ExpiresIn)
            };
        }

        public string Issuer { get; private set; }

        public string ClientId { get; private set; }

        public string OpenId { get; private set; }

        public string NickName { get; private set; }

        public string Avatar { get; private set; }

        public string AccessToken { get; private set; }

        public string RefreshToken { get; private set; }

        public int ExpiresIn { get; private set; }

        private ICollection<Claim> _claims;

        public ICollection<Claim> Claims
        {
            get
            {
                if (_claims == null)
                {
                    _claims = new List<Claim>
                    {
                        BuildClaim(ClaimTypes.Issuer, Issuer),
                        BuildClaim(System.Security.Claims.ClaimTypes.NameIdentifier, OpenId),
                        BuildClaim(ClaimTypes.ClientId, ClientId),
                        BuildClaim(ClaimTypes.OpenId, OpenId),
                        BuildClaim(ClaimTypes.AccessToken, AccessToken),
                        BuildClaim(ClaimTypes.RefreshToken, RefreshToken),
                        BuildClaim(ClaimTypes.ExpiresIn, ExpiresIn.ToString()),
                        BuildClaim(ClaimTypes.NickName, NickName),
                        BuildClaim(ClaimTypes.Avatar, Avatar),
                        BuildClaim("nickname", NickName),
                        BuildClaim("avatar", Avatar)
                };
                }
                return _claims;
            }
            private set => _claims = value;
        }

        public ClaimsIdentity BuildClaimsIdentity()
        {
            return new ClaimsIdentity(
                Claims,
                Issuer,
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        }

        public ClaimsPrincipal BuildClaimsPrincipal()
        {
            return new ClaimsPrincipal(BuildClaimsIdentity());
        }

        private Claim BuildClaim(string type, string value)
        {
            return new Claim(type, value.EmptyIfNull(), ClaimValueTypes.String, Issuer);
        }
    }
}