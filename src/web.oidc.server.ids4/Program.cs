using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ServerSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}