using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace OAuth2.Github.AspNetCore
{
    /// <summary>
    /// <see cref="https://developer.github.com/apps/building-oauth-apps/authorizing-oauth-apps/"/>
    /// </summary>
    public class GithubOAuthOptions : OAuthOptions
    {
        public GithubOAuthOptions()
        {
            base.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            base.TokenEndpoint = "https://github.com/login/oauth/access_token";
            base.UserInformationEndpoint = "https://api.github.com/user";
            base.CallbackPath = new PathString("/oauth2/github/callback");
            base.Scope.Add("user");
            base.SaveTokens = false;
            base.ClaimActions.Add(new GithubClaimsAction());
        }
    }
}