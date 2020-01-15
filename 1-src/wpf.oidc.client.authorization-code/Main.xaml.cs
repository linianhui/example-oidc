using System.Windows;
using Newtonsoft.Json;
using WPFClient.Models;
using WPFClient.Oidc;

namespace WPFClient
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            RefreshUi();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            new Login().ShowDialog();
            RefreshUi();
        }

        private void QQLogin_Click(object sender, RoutedEventArgs e)
        {
            new Login("qq").ShowDialog();
            RefreshUi();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            TokenFile.Delete();
            RefreshUi();
        }

        private void RefreshUi()
        {
            this.DataContext = GetTokenModel();
        }

        private TokenModel GetTokenModel()
        {
            var token = TokenFile.Read();
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
