namespace OAuth2.QQConnect
{
    public static class QQConnectConstants
    {
        public const string DefaultAuthenticationType = "QQ.Connect";

        internal const string AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";

        internal const string AccessTokenEndpoint = "https://graph.qq.com/oauth2.0/token";

        internal const string OpenIdEndpoint = "https://graph.qq.com/oauth2.0/me";

        internal const string UserInfoEndpoint = "https://graph.qq.com/user/get_user_info";
    }
}