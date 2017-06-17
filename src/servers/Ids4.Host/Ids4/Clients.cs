using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Ids4.Host.Ids4
{
    public static class Clients
    {
        public static IEnumerable<Client> All => new[]
        {
            ImplicitClient
        };

        private static Client ImplicitClient
        {
            get
            {
                var host = "http://client.implicit.dev/";

                return new Client
                {
                    Enabled = true,
                    ClientName = "Implicit Client",
                    ClientId = "implicit-client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = new List<string>
                    {
                        host
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        host
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    }
                };
            }
        }
    }
}