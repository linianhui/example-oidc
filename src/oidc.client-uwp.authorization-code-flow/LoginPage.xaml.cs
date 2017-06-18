using System;
using System.IO;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using System.Text;
using Windows.Storage;
using Client.AuthorizationCodeFlow.UWP.Oidc;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client.AuthorizationCodeFlow.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private readonly OidcOptions _oidcOptions = new OidcOptions();

        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                var idp = ((dynamic)e.Parameter)?.idp;
                this.NameLoginWebView.Navigate(new Uri(this._oidcOptions.BuildAuthorizeUrl(idp?.ToString()), UriKind.Absolute));
            }
            base.OnNavigatedTo(e);
        }

        private async void NameLoginWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            var uri = args.Uri;
            if (uri.ToString().StartsWith(this._oidcOptions.RedirectUri))
            {
                var code = OidcOptions.GetCode(uri.Query);
                var httpClient = new HttpClient();
                var tokenParams = new FormUrlEncodedContent(this._oidcOptions.BuildTokenParams(code));
                var tokenReponse = await httpClient.PostAsync(this._oidcOptions.TokenEndpoint, tokenParams);
                var jsonText = await tokenReponse.Content.ReadAsStringAsync();
                //todo:http://openid.net/specs/openid-connect-core-1_0.html#TokenResponseValidation
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("user-token.json", CreationCollisionOption.ReplaceExisting);
                using (var fileStream = await file.OpenStreamForWriteAsync())
                {
                    var jsonBytes = Encoding.UTF8.GetBytes(jsonText);
                    fileStream.Write(jsonBytes, 0, jsonBytes.Length);
                }
                if (this.Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }
            }
        }
    }
}
