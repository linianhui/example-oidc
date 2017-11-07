using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace OAuth2.QQConnect.Core1
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseQQConnectAuthentication(
            this IApplicationBuilder app,
            string clientId,
            string clientSecret)
        {
            return UseQQConnectAuthentication(app, new CoreQQConnectOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            });
        }

        public static IApplicationBuilder UseQQConnectAuthentication(
            this IApplicationBuilder app,
            CoreQQConnectOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<CoreQQConnectMiddleware>(Options.Create(options));
        }
    }
}