using Newtonsoft.Json.Linq;

namespace OAuth2.QQConnect.Models
{
    public sealed class UserModel
    {
        private UserModel() { }

        public string NickName { get; private set; }

        public string Avatar { get; private set; }

        public static UserModel From(string jsonUser)
        {
            var user = JObject.Parse(jsonUser);
            return new UserModel
            {
                NickName = user.TryGetValue("nickname"),
                Avatar = user.TryGetValue("figureurl_qq_1")
            };
        }
    }
}
