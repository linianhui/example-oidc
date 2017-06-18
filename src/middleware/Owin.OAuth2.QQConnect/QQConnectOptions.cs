using Microsoft.Owin.Security;

namespace OAuth2.QQConnect
{
    public class QQConnectOptions : AuthenticationOptions
    {
        public QQConnectOptions()
            : base(QQConnectDefaults.AuthenticationType)
        {
            Caption = QQConnectDefaults.AuthenticationType;
            AuthenticationMode = AuthenticationMode.Passive;

            CallbackPath = QQConnectDefaults.CallbackPath;

            AuthorizationEndpoint = QQConnectDefaults.AuthorizationEndpoint;
            TokenEndpoint = QQConnectDefaults.TokenEndpoint;
            OpenIdEndpoint = QQConnectDefaults.OpenIdEndpoint;
            UserInformationEndpoint = QQConnectDefaults.UserInformationEndpoint;

            Scopes = QQConnectDefaults.Scopes;
        }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string SignInAsAuthenticationType { get; set; }

        public string Caption
        {
            get { return base.Description.Caption; }
            set { base.Description.Caption = value; }
        }

        public string AuthorizationEndpoint { get; set; }

        public string TokenEndpoint { get; set; }

        public string OpenIdEndpoint { get; set; }

        public string UserInformationEndpoint { get; set; }

        public string CallbackPath { get; set; }

        public string[] Scopes { get; set; }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        public string DisplayMode { get; set; }
    }
}