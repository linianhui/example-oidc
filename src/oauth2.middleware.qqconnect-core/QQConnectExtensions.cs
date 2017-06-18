using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Options;
using System;

namespace OAuth2.QQConnect
{
    public static class QQConnectExtensions
    {
        public static IApplicationBuilder UseQQConnectAuthentication(this IApplicationBuilder app, string clientId, string clientSecret)
        {
            return UseQQConnectAuthentication(app, new QQConnectOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            });
        }

        public static IApplicationBuilder UseQQConnectAuthentication(this IApplicationBuilder app, QQConnectOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<QQConnectMiddleware>(Options.Create(options));
        }

        public static AuthenticationProperties SetQQConncetSignInParams(
            this AuthenticationProperties authenticationProperties,
            QQConnectSignInParams qqConnectSignInParams)
        {
            if (authenticationProperties == null)
            {
                throw new ArgumentNullException(nameof(authenticationProperties));
            }
            authenticationProperties.Items.SetQQConncetSignInParams(qqConnectSignInParams);
            return authenticationProperties;
        }

        public static QQConnectSignInParams GetQQConncetSignInParams(this AuthenticationProperties authenticationProperties)
        {
            if (authenticationProperties == null)
            {
                throw new ArgumentNullException(nameof(authenticationProperties));
            }

            return authenticationProperties.Items.GetQQConncetSignInParams();
        }

        public static void RemoveQQConncetSignInParams(this AuthenticationProperties authenticationProperties)
        {
            if (authenticationProperties == null)
            {
                throw new ArgumentNullException(nameof(authenticationProperties));
            }
            authenticationProperties.Items.RemoveQQConncetSignInParams();
        }
    }
}