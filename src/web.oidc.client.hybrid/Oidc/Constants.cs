namespace ClientSite.Oidc
{
    public class Constants
    {
        public static readonly string AuthenticationSchemeOfCookies = "authc.cookies";
        public static readonly string AuthenticationSchemeOfOidc = "authc.oidc";
        public static readonly string CookieName = "lnh.oidc";

        public class ClaimTypes
        {
            public static readonly string Subject = "sub";
            public static readonly string IdToken = "id_token";
            public static readonly string SessionId = "sid";
        }
    }
}