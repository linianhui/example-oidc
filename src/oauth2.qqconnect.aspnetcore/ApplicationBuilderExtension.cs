using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OAuth2.QQConnect.AspNetCore
{
    public static class ApplicationBuilderExtension
    {
        public static AuthenticationBuilder AddQQConnect(
            this AuthenticationBuilder builder,
            string scheme,
            string displayName,
            Action<QQConnectOAuthOptions> configureOptions)
        {
            return builder.AddOAuth<QQConnectOAuthOptions, QQConnectOAuthHandler>(
                  scheme,
                  displayName,
                  configureOptions);
        }
    }
}