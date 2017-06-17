using Microsoft.Owin.Security;
using System.Collections.Generic;

namespace OAuth2.QQConnect
{
    public class QQConnectAuthenticationOptions : AuthenticationOptions
    {
        public QQConnectAuthenticationOptions()
            : base(QQConnectConstants.DefaultAuthenticationType)
        {
            Caption = QQConnectConstants.DefaultAuthenticationType;
            AuthenticationMode = AuthenticationMode.Passive;
            CallbackPath = "/qq-connect/callback";
            AuthorizationEndpoint = QQConnectConstants.AuthorizationEndpoint;
            AccessTokenEndpoint = QQConnectConstants.AccessTokenEndpoint;
            OpenIdEndpoint = QQConnectConstants.OpenIdEndpoint;
            UserInfoEndpoint = QQConnectConstants.UserInfoEndpoint;
            Scopes = new List<string>
            {
                "get_user_info"
            };
        }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string SignInAsAuthenticationType { get; set; }

        public string Caption
        {
            get { return base.Description.Caption; }
            set { base.Description.Caption = value; }
        }

        public string AuthorizationEndpoint { get; set; }

        public string AccessTokenEndpoint { get; set; }

        public string OpenIdEndpoint { get; set; }

        public string UserInfoEndpoint { get; set; }

        public string CallbackPath { get; set; }

        public List<string> Scopes { get; set; }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        public string DisplayMode { get; set; }
    }
}