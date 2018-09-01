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

        public override void Run(JObject userData, ClaimsIdentity identity, string issuer)
        {
            if (userData != null)
            {
                foreach (var userDatum in userData)
                {
                    var key = userDatum.Key;
                    var value = userDatum.Value.ToString();
                    identity.AddClaim(new Claim("github." + key, value, ClaimValueTypes.String, issuer));
                }
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userData.GetValue("id").ToString(), ClaimValueTypes.String, issuer));
            }
        }
    }
}
