using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ClientCredentials
{
    class Program
    {
        public static void Main()
        {
            DiagnosticListener.AllListeners.Subscribe(new DiagnosticListenerObserver());
            MainAsync().GetAwaiter().GetResult();
            Console.WriteLine("ok");
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            var tokenResponse = await GetToken();
            var client = new HttpClient();

            client.SetBearerToken(tokenResponse.AccessToken);

            var apis = new List<string>
            {
                "http://oauth2-resources-aspnetcore.test",
                "http://oauth2-resources-aspnetcore.test/books",
                "http://oauth2-resources-nodejs.test",
                "http://oauth2-resources-owin.test"
            };

            foreach (var api in apis)
            {
                try
                {
                    await client.GetAsync(api);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static async Task<TokenResponse> GetToken()
        {
            var discoveryClient = new DiscoveryClient("http://oidc-server.test")
            {
                Policy =
                {
                    RequireHttps = false
                }
            };
            var discoveryResponse = await discoveryClient.GetAsync();

            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "client-credentials-client", "lnh");

            return await tokenClient.RequestClientCredentialsAsync("api-1 api-2 api-3");
        }
    }
}
