using Owin;
using System;

namespace OAuth2.QQConnect.Owin
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseQQConnectAuthentication(
            this IAppBuilder app,
            string clientId,
            string clientSercet)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseQQConnectAuthentication(new OwinQQConnectOptions
            {
                ClientId = clientId,
                ClientSecret = clientSercet
            });
        }

        public static IAppBuilder UseQQConnectAuthentication(
            this IAppBuilder app,
            OwinQQConnectOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.Use(typeof(OwinQQConnectMiddleware), app, options);
        }
    }
}