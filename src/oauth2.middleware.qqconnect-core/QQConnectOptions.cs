using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace OAuth2.QQConnect
{
    public class QQConnectOptions : OAuthOptions
    {
        public QQConnectOptions()
        {
            base.AuthenticationScheme = QQConnectDefaults.AuthenticationType;

            base.CallbackPath = new PathString(QQConnectDefaults.CallbackPath);

            base.AuthorizationEndpoint = QQConnectDefaults.AuthorizationEndpoint;
            base.TokenEndpoint = QQConnectDefaults.TokenEndpoint;
            this.OpenIdEndpoint = QQConnectDefaults.OpenIdEndpoint;
            base.UserInformationEndpoint = QQConnectDefaults.UserInformationEndpoint;

            foreach (var scope in QQConnectDefaults.Scopes)
            {
                base.Scope.Add(scope);
            }
        }

        public string OpenIdEndpoint { get; set; }

        public string DisplayMode { get; set; }
    }
}