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
            AuthorizationCodeClient
        };

        private static Client ImplicitClient
        {
            get
            {
                const string home = "http://oidc-client-implicit.dev";


                return new Client
                {
                    ClientId = "implicit-client",
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
                const string home = "http://oidc-client-hybrid.dev";

                return new Client
                {
                    ClientId = "hybrid-client",
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
                const string host = "http://oidc-client-js.dev";
                return new Client
                {
                    ClientId = "js-client",
                    ClientName = "JS Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris =
                    {
                        $"{host}/login.html",
                        $"{host}/refresh-token.html"
                    },
                    PostLogoutRedirectUris = { $"{host}/index.html" },

                    AllowedCorsOrigins = { host },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
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
                const string home = "http://oidc-client-authorization-code.dev";

                return new Client
                {
                    ClientId = "oidc-authorization-code-client",
                    ClientName = "Oidc Authorization Code Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("lnh".Sha256())
                    },

                    RequireConsent = true,
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
    }
}