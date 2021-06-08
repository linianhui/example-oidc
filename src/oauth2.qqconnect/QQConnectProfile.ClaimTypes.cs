namespace OAuth2.QQConnect
{
    public sealed partial class QQConnectProfile
    {
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