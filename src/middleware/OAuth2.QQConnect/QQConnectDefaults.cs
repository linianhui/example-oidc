namespace OAuth2.QQConnect
{
    public static class QQConnectDefaults
    {
        public static readonly string AuthenticationType = "QQ.Connect";

        public static readonly string ClaimTypePrefix = "qq.";

        internal static readonly string AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";

        internal static readonly string TokenEndpoint = "https://graph.qq.com/oauth2.0/token";

        internal static readonly string OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";

        internal static readonly string UserInformationEndpoint = "https://graph.qq.com/user/get_user_info";

        internal static readonly string CallbackPath = "/qq-connect/callback";

        internal static readonly string[] Scopes = new string[] { "get_user_info" };

        internal static readonly string ErrorCodeField = "ret";

        internal static readonly string OpenIdField = "openid";

        internal static readonly string AccessTokenField = "access_token";

        internal static readonly string RefreshTokenField = "refresh_token";

        internal static readonly string ExpiresInField = "expires_in";

        internal static readonly string NickNameField = "nickname";

        internal static readonly string AvatarUrlField = "figureurl_qq_1";

        internal static readonly string IdpClaimType = ClaimTypePrefix + "idp";

        internal static readonly string ClientIdClaimType = ClaimTypePrefix + "clientId";

        internal static readonly string OpenIdClaimType = ClaimTypePrefix + OpenIdField;

        internal static readonly string AccessTokenClaimType = ClaimTypePrefix + AccessTokenField;

        internal static readonly string RefreshTokenClaimType = ClaimTypePrefix + RefreshTokenField;

        internal static readonly string ExpiresInClaimType = ClaimTypePrefix + ExpiresInField;

        internal static readonly string NickNameClaimType = ClaimTypePrefix + NickNameField;

        internal static readonly string AvatarUrlClaimType = ClaimTypePrefix + AvatarUrlField;
    }
}