using Microsoft.Owin.Security;
using Owin;
using System;

namespace OAuth2.QQConnect
{
    public static class QQConnectExtensions
    {
        public static IAppBuilder UseQQConnectAuthentication(this IAppBuilder app, string clientId, string clientSecret)
        {
            return UseQQConnectAuthentication(app, new QQConnectOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
            });
        }

        public static IAppBuilder UseQQConnectAuthentication(this IAppBuilder app, QQConnectOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.Use(typeof(QQConnectMiddleware), app, options);
            return app;
        }

        public static AuthenticationProperties SetQQConncetSignInParams(
            this AuthenticationProperties authenticationProperties,
            QQConnectSignInParams qqConnectSignInParams)
        {
            if (authenticationProperties == null)
            {
                throw new ArgumentNullException(nameof(authenticationProperties));
            }
            authenticationProperties.Dictionary.SetQQConncetSignInParams(qqConnectSignInParams);
            return authenticationProperties;
        }

        public static QQConnectSignInParams GetQQConncetSignInParams(this AuthenticationProperties authenticationProperties)
        {
            if (authenticationProperties == null)
            {
                throw new ArgumentNullException(nameof(authenticationProperties));
            }

            return authenticationProperties.Dictionary.GetQQConncetSignInParams();
        }

        public static void RemoveQQConncetSignInParams(this AuthenticationProperties authenticationProperties)
        {
            if (authenticationProperties == null)
            {
                throw new ArgumentNullException(nameof(authenticationProperties));
            }
            authenticationProperties.Dictionary.RemoveQQConncetSignInParams();
        }
    }
}