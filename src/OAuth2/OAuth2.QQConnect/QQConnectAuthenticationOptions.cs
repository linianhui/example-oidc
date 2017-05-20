using Microsoft.Owin.Security;

namespace OAuth2.QQConnect
{
    public class QQConnectAuthenticationOptions : AuthenticationOptions
    {
        public QQConnectAuthenticationOptions()
            : base(Constants.DefaultAuthenticationType)
        {
            Caption = Constants.DefaultAuthenticationType;
            AuthenticationMode = AuthenticationMode.Passive;
        }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string SignInAsAuthenticationType { get; set; }

        public string Caption
        {
            get { return base.Description.Caption; }
            set { base.Description.Caption = value; }
        }
    }
}