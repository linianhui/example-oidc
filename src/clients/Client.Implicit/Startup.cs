using Client.Implicit.Oidc;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Client.Implicit.Startup))]

namespace Client.Implicit
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseBaseAuthentication();
            app.UseIds3Authentication();
            app.UseIds4Authentication();
        }
    }
}