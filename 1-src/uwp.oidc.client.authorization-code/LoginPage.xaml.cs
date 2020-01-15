using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using UWPClient.Oidc;

namespace UWPClient
{
    public sealed partial class LoginPage : Page
    {
        private readonly OidcClient _oidcClient = new OidcClient();

        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                var idp = ((dynamic)e.Parameter)?.idp;
                this.NameLoginWebView.Navigate(new Uri(this._oidcClient.BuildAuthorizeUrl(idp?.ToString()), UriKind.Absolute));
            }
            base.OnNavigatedTo(e);
        }

        private async void NameLoginWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            var uri = args.Uri;
            if (uri.ToString().StartsWith(OidcClient.Options.RedirectUri))
            {
                var code = OidcClient.GetCode(uri.Query);
                var token = await this._oidcClient.GetTokenAsync(code);
                await TokenFile.WriteAsync(token);
                if (this.Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }
            }
        }
    }
}
