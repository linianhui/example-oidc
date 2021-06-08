using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace OAuth2.Github.AspNetCore
{
    public static class ApplicationBuilderExtension
    {
        public static AuthenticationBuilder AddGithub(
            this AuthenticationBuilder @this,
            string scheme,
            string displayName,
            Action<GithubOAuthOptions> configureOptions)
        {
            return @this.AddOAuth<GithubOAuthOptions, GithubOAuthHandler>(scheme, displayName, configureOptions);
        }
    }
}