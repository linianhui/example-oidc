namespace Client.Implicit.Oidc
{
    public class Constants
    {
        public static readonly string AuthenticationTypeOfCookies = ".lnh";
        public static readonly string AuthenticationTypeOfOidc = ".oidc";

        public class ClaimTypes
        {
            public static readonly string Subject = "sub";
            public static readonly string IdToken = "id_token";
            public static readonly string SessionId = "sid";
        }
    }
}