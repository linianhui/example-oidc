using ClientSite;
using ClientSite.OAuth2;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace ClientSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseQQConnect();
        }
    }
}
