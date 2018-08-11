namespace ClientSite.Oidc
{
    public class Constants
    {
        public static readonly string AuthenticationTypeOfCookies = "authc.cookies";
        public static readonly string AuthenticationTypeOfOidc = "authc.oidc";
        public static readonly string CookieName = "lnh.oidc";

        public class ClaimTypes
        {
            public static readonly string Subject = "sub";
            public static readonly string IdToken = "id_token";
            public static readonly string SessionId = "sid";
        }
    }
}