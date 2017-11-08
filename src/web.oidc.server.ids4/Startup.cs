using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OAuth2.QQConnect.AspNetCore;
using ServerSite.Ids4;

namespace ServerSite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddIds4();

            services.AddAuthentication()
                  .AddQQConnect(options =>
                  {
                      options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                      options.ClientId = QQConnectConfig.ClientId;
                      options.ClientSecret = QQConnectConfig.ClientSecret;
                  });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseDeveloperExceptionPage();

            app.UseIds4();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}