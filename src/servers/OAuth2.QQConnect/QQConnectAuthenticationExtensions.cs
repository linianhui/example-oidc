using Owin;
using System;

namespace OAuth2.QQConnect
{
    public static class QQConnectAuthenticationExtensions
    {
        public static IAppBuilder UseQQConnectAuthentication(this IAppBuilder app, string appId, string appSecret)
        {
            return UseQQConnectAuthentication(app, new QQConnectAuthenticationOptions
            {
                AppId = appId,
                AppSecret = appSecret,
            });
        }

        public static IAppBuilder UseQQConnectAuthentication(this IAppBuilder app, QQConnectAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.Use(typeof(QQConnectAuthenticationMiddleware), app, options);
            return app;
        }
    }
}