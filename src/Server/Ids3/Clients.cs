using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace Server.Ids3
{
    public static class Clients
    {
        public static IEnumerable<Client> All => new[]
        {
            ImplicitClient
        };

        private static Client ImplicitClient => new Client
        {
            Enabled = true,
            ClientName = "Implicit Client",
            ClientId = "implicit-client",
            Flow = Flows.Implicit,
            RedirectUris = new List<string>
            {
                "http://client.implicit.dev/"
            },
            AllowAccessToAllScopes = true
        };
    }
}