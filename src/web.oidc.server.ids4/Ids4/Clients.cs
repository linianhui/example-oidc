using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace ServerSite.Ids4
{
    public static class Clients
    {
        public static IEnumerable<Client> All => new[]
        {
            HybridClient,
            ImplicitClient,
            JSClient,
            OAuth2AuthorizationCodeFlowClientForUWP
        };

        private static Client ImplicitClient
        {
            get
            {
                var host = "http://oidc-client-implicit.dev";

                return new Client
                {
                    Enabled = true,
                    ClientName = "Implicit Client",
                    ClientId = "implicit-client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    RedirectUris = new List<string>
                    {
                        host + "/"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        host + "/"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    FrontChannelLogoutUri = host + "/account/logout-callback",
                    FrontChannelLogoutSessionRequired = true
                };
            }
        }

        private static Client HybridClient
        {
            get
            {
                var host = "http://oidc-client-hybrid.dev";

                return new Client
                {
                    Enabled = true,
                    ClientName = "AuthorizationCode Client",
                    ClientId = "authorization-code-client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("lnh".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent = false,
                    RedirectUris = new List<string>
                    {
                        host + "/oidc/signin-callback"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        host + "/"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    FrontChannelLogoutUri = host + "/oidc/signout-callback",
                    FrontChannelLogoutSessionRequired = true
                };
            }
        }

        private static Client JSClient
        {
            get
            {
                var host = "http://oidc-client-js.dev";
                return new Client
                {
                    Enabled = true,
                    ClientName = "JS Client",
                    ClientId = "js-client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = new List<string>
                    {
                        $"{host}/login.html",
                        $"{host}/refresh-token.html"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{host}/index.html"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        host
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    AccessTokenLifetime = 600,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                };
            }
        }

        private static Client OAuth2AuthorizationCodeFlowClientForUWP
        {
            get
            {
                var host = "http://uwp.oauth2-authorization-code-flow.dev";

                return new Client
                {
                    Enabled = true,
                    ClientName = "OAuth2 Authorization Code Flow Client",
                    ClientId = "oauth2-authorization-code-flow.uwp",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("lnh".Sha256())
                    },
                    RequireConsent = true,
                    AllowRememberConsent = true,
                    ClientUri = host,
                    RedirectUris = new List<string>
                    {
                        $"{host}/oauth2-callback",
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },
                };
            }
        }
    }
}