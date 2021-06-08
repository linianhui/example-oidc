using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Http.Diagnostic;
using IdentityModel.Client;

namespace ResourceOwnerPasswordCredentials
{
    class Program
    {
        private static HttpClient httpClient = new HttpClient();

        public static async Task Main()
        {
            DiagnosticListener.AllListeners.Subscribe(new HttpDiagnosticListenerObserver());
            await MainAsync();
            Console.WriteLine("ok");
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            var tokenResponse = await GetTokenAsync();

            httpClient.SetBearerToken(tokenResponse.AccessToken);

            var apis = new List<string>
            {
                "http://oauth2-resources-aspnetcore.test",
                "http://oauth2-resources-aspnetcore.test/books",
                "http://oauth2-resources-nodejs.test",
                "http://oauth2-resources-java.test",
                "http://oauth2-resources-owin.test"
            };

            foreach (var api in apis)
            {
                try
                {
                    await httpClient.GetAsync(api);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static async Task<TokenResponse> GetTokenAsync()
        {
            var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://oidc-server.test",
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            return await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = "resource-owner-password-credentials-client",
                UserName = "lnh",
                Password = "123",
                Scope = "api-1 api-2 api-3"
            });
        }
    }
}
