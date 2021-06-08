using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace OAuth2.Github.AspNetCore
{
    public class GithubClaimsAction : ClaimAction
    {
        public GithubClaimsAction()
            : base("github", ClaimValueTypes.String)
        {
        }

        public override void Run(JsonElement user, ClaimsIdentity identity, string issuer)
        {
            foreach (var item in user.EnumerateObject())
            {
                var key = item.Name;
                var value = item.Value.ToString();
                identity.AddClaim(new Claim("github." + key, value, ClaimValueTypes.String, issuer));
            }

            var userId = user.GetProperty("id").ToString();
            var userName = user.GetProperty("name").ToString();
            var userAvatar = user.GetProperty("avatar_url").ToString();

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, issuer));
            identity.AddClaim(new Claim("nickname", userName, ClaimValueTypes.String, issuer));
            identity.AddClaim(new Claim("avatar", userAvatar, ClaimValueTypes.String, issuer));
        }
    }
}
