using System;
using Microsoft.Owin.Security;
using OAuth2.QQConnect.Basic;

namespace OAuth2.QQConnect.Owin
{
    public class OwinQQConnectOptions : AuthenticationOptions
    {
        public OwinQQConnectOptions()
            : base("qq.connect")
        {
            DisplayName = "QQ Connect";
            AuthenticationMode = AuthenticationMode.Passive;
        }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public bool IsMobile { get; set; } = false;

        public string[] Scopes { get; set; } = { "get_user_info" };

        public string CallbackPath { get; set; } = "/oauth2/qq-connect/callback";

        public string DisplayName
        {
            get => Description.Caption;
            set => Description.Caption = value;
        }

        public string SignInAsAuthenticationType { get; set; }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        internal QQConnectOptions BuildQQConnectOptions(Func<string> redirectUrl)
        {
            return new QQConnectOptions(ClientId, ClientSecret, IsMobile, Scopes)
            {
                RedirectUrl = redirectUrl
            };
        }
    }
}