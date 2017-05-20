using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Owin;
using System;

namespace OAuth2.QQConnect
{
    public class QQConnectAuthenticationMiddleware : AuthenticationMiddleware<QQConnectAuthenticationOptions>
    {
        public QQConnectAuthenticationMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            QQConnectAuthenticationOptions options)
            : base(next, options)
        {
        }

        protected override AuthenticationHandler<QQConnectAuthenticationOptions> CreateHandler()
        {
            throw new NotImplementedException();
        }
    }
}