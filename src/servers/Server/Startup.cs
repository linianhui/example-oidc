using Microsoft.Owin;
using Owin;
using Server.Ids3.Use;

[assembly: OwinStartup(typeof(Server.Startup))]

namespace Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIds3();
        }
    }
}