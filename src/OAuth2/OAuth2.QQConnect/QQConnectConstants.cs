namespace OAuth2.QQConnect
{
    public static class QQConnectConstants
    {
        public const string DefaultAuthenticationType = "QQ.Connect";

        public const string ClaimTypePrefix = "qq.";

        internal const string AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";

        internal const string AccessTokenEndpoint = "https://graph.qq.com/oauth2.0/token";

        internal const string OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";

        internal const string UserInfoEndpoint = "https://graph.qq.com/user/get_user_info";

        internal const string ErrorCodeField = "ret";

        internal const string OpenIdField = "openid";

        internal const string AccessTokenField = "access_token";

        internal const string RefreshTokenField = "refresh_token";

        internal const string ExpiresInField = "expires_in";

        internal const string NickNameField = "nickname";

        internal const string AvatarUrlField = "figureurl_qq_1";

        internal const string IdpClaimType = ClaimTypePrefix + "idp";

        internal const string AppIdClaimType = ClaimTypePrefix + "appid";

        internal const string OpenIdClaimType = ClaimTypePrefix + OpenIdField;

        internal const string AccessTokenClaimType = ClaimTypePrefix + AccessTokenField;

        internal const string RefreshTokenClaimType = ClaimTypePrefix + RefreshTokenField;

        internal const string ExpiresInClaimType = ClaimTypePrefix + ExpiresInField;

        internal const string NickNameClaimType = ClaimTypePrefix + NickNameField;

        internal const string AvatarUrlClaimType = ClaimTypePrefix + AvatarUrlField;
    }
}