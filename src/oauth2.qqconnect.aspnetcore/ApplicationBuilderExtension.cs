using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace OAuth2.QQConnect.AspNetCore
{
    public static class ApplicationBuilderExtension
    {
        public static AuthenticationBuilder AddQQConnectAuthentication(
            this AuthenticationBuilder builder,
            Action<QQConnectOAuthOptions> configureOptions,
            string scheme = "qq.connect",
            string displayName = "QQ Connect")
        {
            return builder.AddOAuth<QQConnectOAuthOptions, QQConnectOAuthHandler>(
                  scheme,
                  displayName,
                  configureOptions);
        }
    }
}