using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Newtonsoft.Json.Linq;

namespace OAuth2.Github.AspNetCore
{
    public class GithubClaimsAction : ClaimAction
    {
        public GithubClaimsAction()
            : base("github", ClaimValueTypes.String)
        {
        }

        public override void Run(JObject user, ClaimsIdentity identity, string issuer)
        {
            if (user == null)
            {
                return;
            }

            foreach (var item in user)
            {
                var key = item.Key;
                var value = item.Value.ToString();
                identity.AddClaim(new Claim("github." + key, value, ClaimValueTypes.String, issuer));
            }

            var userId = user.GetValue("id").ToString();
            var userName = user.GetValue("name").ToString();
            var userAvatar = user.GetValue("avatar_url").ToString();

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, issuer));
            identity.AddClaim(new Claim("nickname", userName, ClaimValueTypes.String, issuer));
            identity.AddClaim(new Claim("avatar", userAvatar, ClaimValueTypes.String, issuer));
        }
    }
}
