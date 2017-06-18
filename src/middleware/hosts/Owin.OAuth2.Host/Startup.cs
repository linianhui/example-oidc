using Microsoft.Owin;
using Owin.OAuth2.Host;
using Owin.OAuth2.Host.OAuth2;

[assembly: OwinStartup(typeof(Startup))]

namespace Owin.OAuth2.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseQQConnect();
        }
    }
}
