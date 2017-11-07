namespace ClientSite.Oidc
{
    public class Constants
    {
        public static readonly string AuthenticationTypeOfCookies = ".lnh";
        public const string AuthenticationTypeOfIds3 = ".ids3";
        public static readonly string AuthenticationTypeOfIds4 = ".ids4";

        public class ClaimTypes
        {
            public static readonly string Subject = "sub";
            public static readonly string IdToken = "id_token";
            public static readonly string SessionId = "sid";
        }
    }
}