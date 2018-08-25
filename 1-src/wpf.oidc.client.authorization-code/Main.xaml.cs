using System.IdentityModel.Tokens.Jwt;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            App.DeleteToken();
            RefreshUi();
        }

        private void RefreshUi()
        {
            this.DataContext = GetTokenModel();
        }

        private TokenModel GetTokenModel()
        {
            var token = App.ReadToken();
            if (token == null)
            {
                return new TokenModel();
            }

            return BuildTokenModel(token);
        }

        private TokenModel BuildTokenModel(JToken token)
        {
            return new TokenModel
            {
                Token = token.ToString(Formatting.Indented),
                IdToken = Jwt.From(token.Value<string>("id_token")),
                AccessToken = Jwt.From(token.Value<string>("access_token")),
            };
        }

        private JObject JwtToJson(JwtSecurityToken jwt)
        {
            var json = new JObject();
            foreach (var claim in jwt.Claims)
            {
                json.Add(claim.Type, claim.Value);
            }
            return json;
        }

        public class Jwt
        {
            public object header { get; set; }
            public object payload { get; set; }
            public string signature { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }

            public static Jwt From(string jwtString)
            {
                var jwt = new JwtSecurityToken(jwtString);
                return new Jwt
                {
                    header = jwt.Header,
                    payload = jwt.Payload,
                    signature = jwt.RawSignature
                };
            }
        }

        public class TokenModel
        {
            public string Token { get; set; }

            public Jwt IdToken { get; set; }

            public Jwt AccessToken { get; set; }

            public Visibility LoginVisibility
            {
                get
                {
                    if (Token == null)
                    {
                        return Visibility.Visible;
                    }
                    return Visibility.Collapsed;
                }
            }

            public Visibility LogoutVisibility
            {
                get
                {
                    if (Token == null)
                    {
                        return Visibility.Collapsed;
                    }
                    return Visibility.Visible;
                }
            }
        }
    }
}
