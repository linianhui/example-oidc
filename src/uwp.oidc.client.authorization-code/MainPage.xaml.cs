using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using UWPClient.Models;
using UWPClient.Oidc;
using Windows.UI.Xaml;

namespace UWPClient
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await RefreshUi();
            base.OnNavigatedTo(e);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        private void QQLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage), new { idp = "qq" });
        }

        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            await TokenFile.DeleteAsync();
            await RefreshUi();
        }

        private async Task RefreshUi()
        {
            this.DataContext = await GetTokenModel();
        }

        private async Task<TokenModel> GetTokenModel()
        {
            var token = await TokenFile.ReadAsync();
            if (token == null)
            {
                return new TokenModel();
            }

            return new TokenModel
            {
                Token = token.ToString(Formatting.Indented),
                IdToken = JwtModel.From(token.Value<string>("id_token")),
                AccessToken = JwtModel.From(token.Value<string>("access_token")),
            };
        }
    }
}