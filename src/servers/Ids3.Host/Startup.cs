using Ids3.Host;
using Ids3.Host.Ids3.Use;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Ids3.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIds3();
        }
    }
}