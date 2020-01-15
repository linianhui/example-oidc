namespace WPFClient.Oidc
{
    public class OidcOptions
    {
        public string ClientId => "oidc-client-authorization-code.test";
        public string ClientSecret => "lnh";
        public string AuthorizeEndpoint => "http://oidc-server.test/connect/authorize";
        public string TokenEndpoint => "http://oidc-server.test/connect/token";
        public string RedirectUri => "http://oidc-client-authorization-code.test/oidc/login-callback";
    }
}