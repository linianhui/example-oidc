using System.IO;
using System.Windows;
using System.Windows.Navigation;
using WPFClient.Oidc;

namespace WPFClient
{
    public partial class Login : Window
    {
        private readonly OidcClient _oidcClient = new OidcClient();

        public string Idp { get; }

        public Login(string idp = null)
        {
            Idp = idp;
            InitializeComponent();
            this.NameWebBrowser.Navigate(this._oidcClient.BuildAuthorizeUrl(Idp));
        }

        private async void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var uri = e.Uri;
            if (uri.ToString().StartsWith(OidcClient.Options.RedirectUri))
            {
                var code = OidcClient.GetCode(uri.Query);
                var token = await _oidcClient.GetTokenAsync(code);
                TokenFile.Write(token);
                this.Close();
            }
        }
    }
}
