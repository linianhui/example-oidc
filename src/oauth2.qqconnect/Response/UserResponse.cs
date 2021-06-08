using Newtonsoft.Json.Linq;
using OAuth2.QQConnect.Extensions;

namespace OAuth2.QQConnect.Response
{
    public sealed class UserResponse
    {
        private UserResponse()
        {
        }

        public string NickName { get; private set; }

        public string Avatar { get; private set; }

        public static UserResponse From(string userJson)
        {
            var user = JObject.Parse(userJson);
            return new UserResponse
            {
                NickName = user.TryGetValue("nickname"),
                Avatar = user.TryGetValue("figureurl_qq_1")
            };
        }
    }
}
