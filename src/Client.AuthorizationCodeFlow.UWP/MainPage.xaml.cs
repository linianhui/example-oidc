using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Client.AuthorizationCodeFlow.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var userJson = await ReadUserJson();
            if (userJson != null)
            {
                ShowUser(userJson);
            }
            base.OnNavigatedTo(e);
        }

        private void ShowUser(JToken userJson)
        {
            this.NameUserTextBlock.Text = userJson.ToString(Formatting.Indented);

            var idTokenText = userJson.Value<string>("id_token");
            var idTokenJwt = new JwtSecurityToken(idTokenText);
            var idTokenJson = JwtToJson(idTokenJwt);
            this.NameIdTokenTextBox.Text = idTokenJson.ToString(Formatting.Indented);

            var accessTokenText = userJson.Value<string>("access_token");
            var accessTokenJwt = new JwtSecurityToken(accessTokenText);
            var accessTokenJson = JwtToJson(accessTokenJwt);
            this.NameAccessTokenTextBox.Text = accessTokenJson.ToString(Formatting.Indented);
        }

        private void Login_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        private void QQLogin_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage), new { idp = "qq" });
        }

        public async Task<JObject> ReadUserJson()
        {
            try
            {
                using (var fileStream =
                    await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("user-token.json"))
                {
                    var jsonBytes = new byte[fileStream.Length];
                    fileStream.Read(jsonBytes, 0, jsonBytes.Length);
                    var jsonText = Encoding.UTF8.GetString(jsonBytes);
                    return JObject.Parse(jsonText);
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            return null;
        }

        public JObject JwtToJson(JwtSecurityToken jwt)
        {
            var json = new JObject();
            foreach (var claim in jwt.Claims)
            {
                json.Add(claim.Type, claim.Value);
            }
            return json;
        }
    }
}