using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Client.AuthorizationCodeFlow.UWP.Oidc;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client.AuthorizationCodeFlow.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private readonly Ids3Options _ids3Options = new Ids3Options();

        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                var idp = ((dynamic)e.Parameter)?.idp;
                this.NameLoginWebView.Navigate(new Uri(this._ids3Options.BuildAuthorizeUrl(idp?.ToString()), UriKind.Absolute));
            }
            base.OnNavigatedTo(e);
        }

        private void NameLoginWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            var uri = args.Uri;
            if (uri.ToString().StartsWith(this._ids3Options.RedirectUri))
            {
                var code = Ids3Options.GetCode(uri.Query);
            }
        }


    }
}
