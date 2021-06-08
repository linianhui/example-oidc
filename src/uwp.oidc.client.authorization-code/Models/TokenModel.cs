using Windows.UI.Xaml;

namespace UWPClient.Models
{
    public class TokenModel
    {
        public string Token { get; set; }

        public JwtModel IdToken { get; set; }

        public JwtModel AccessToken { get; set; }

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
