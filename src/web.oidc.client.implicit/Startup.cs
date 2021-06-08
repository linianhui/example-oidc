using ClientSite;
using ClientSite.Oidc;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace ClientSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseOidcAuthentication();
        }
    }
}