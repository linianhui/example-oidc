using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace ClientCredentials
{
    class Program
    {
        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            var discoveryClient = new DiscoveryClient("http://oidc-server.dev")
            {
                Policy =
                {
                    RequireHttps = false
                }
            };

            var discoveryResponse = await discoveryClient.GetAsync();
            if (discoveryResponse.IsError)
            {
                Console.WriteLine(discoveryResponse.Error);
                return;
            }

            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "client-credentials-client", "lnh");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("my-api");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://oauth2-protected-resources.dev/values");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JObject.Parse(content));
            }
        }
    }
}
