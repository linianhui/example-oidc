using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace ServerSite.Ids4
{
    public static class Clients
    {
        private const string OidcLoginCallback = "/oidc/login-callback";
        private const string OidcFrontChannelLogoutCallback = "/oidc/front-channel-logout-callback";

        public static IEnumerable<Client> All => new[]
        {
            HybridClient,
            ImplicitClient,
            JsClient,
            AuthorizationCodeClient,
            ClientCredentialsClient
        };

        private static Client ImplicitClient
        {
            get
            {
                const string home = "http://oidc-client-implicit.test";


                return new Client
                {
                    ClientId = "oidc-client-implicit.test",
                    ClientName = "Implicit Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RequireConsent = false,

                    RedirectUris = { home + OidcLoginCallback },
                    PostLogoutRedirectUris = { home },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },

                    FrontChannelLogoutUri = home + OidcFrontChannelLogoutCallback,
                    FrontChannelLogoutSessionRequired = true
                };
            }
        }

        private static Client HybridClient
        {
            get
            {
                const string home = "http://oidc-client-hybrid.test";

                return new Client
                {
                    ClientId = "oidc-client-hybrid.test",
                    ClientName = "Hybrid Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("lnh".Sha256())
                    },

                    RequireConsent = false,

                    RedirectUris = { home + OidcLoginCallback },
                    PostLogoutRedirectUris = { home },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },

                    FrontChannelLogoutUri = home + OidcFrontChannelLogoutCallback,
                    FrontChannelLogoutSessionRequired = true
                };
            }
        }

        private static Client JsClient
        {
            get
            {
                const string host = "http://oidc-client-js.test";
                return new Client
                {
                    ClientId = "oidc-client-js.test",
                    ClientName = "JS Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris =
                    {
                        $"{host}/oidc/login-callback.html",
                        $"{host}/oidc/refresh-token.html"
                    },
                    PostLogoutRedirectUris = { $"{host}/index.html" },

                    AllowedCorsOrigins = { host },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "api-1",
                        "api-2",
                        "api-3"
                    },
                    AccessTokenLifetime = 3600,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                };
            }
        }

        private static Client AuthorizationCodeClient
        {
            get
            {
                const string home = "http://oidc-client-authorization-code.test";

                return new Client
                {
                    ClientId = "oidc-client-authorization-code.test",
                    ClientName = "Oidc Authorization Code Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("lnh".Sha256())
                    },

                    RequireConsent = false,
                    AllowRememberConsent = true,

                    RedirectUris = { home + OidcLoginCallback },
                    PostLogoutRedirectUris = { home },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },

                    FrontChannelLogoutUri = home + OidcFrontChannelLogoutCallback,
                    FrontChannelLogoutSessionRequired = true
                };
            }
        }


        private static Client ClientCredentialsClient => new Client
        {
            ClientId = "client-credentials-client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("lnh".Sha256()) },
            AllowedScopes = { "api-1", "api-2", "api-3" }
        };
    }
}