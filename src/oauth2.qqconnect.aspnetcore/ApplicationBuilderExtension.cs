using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace OAuth2.QQConnect.AspNetCore
{
    public static class ApplicationBuilderExtension
    {
        public static AuthenticationBuilder AddQQConnect(
            this AuthenticationBuilder builder,
            Action<QQConnectOptions> configureOptions,
            string scheme = "qq.connect",
            string displayName = "QQ Connect")
        {
            return builder.AddOAuth<QQConnectOptions, QQConnectHandler>(
                  scheme,
                  displayName,
                  configureOptions);
        }
    }
}