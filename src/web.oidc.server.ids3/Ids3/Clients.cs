using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace ServerSite.Ids3
{
    public static class Clients
    {
        public static IEnumerable<Client> All => new[]
        {
            ImplicitClient,
            JSClient,
            OAuth2AuthorizationCodeFlowClientForUWP
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
                    Flow = Flows.Implicit,
                    RedirectUris = new List<string>
                    {
                        host
                    },
                    //退出登录后重定向的地址。
                    PostLogoutRedirectUris = new List<string>
                    {
                        host
                    },
                    //禁止退出登录的确认提示页面，直接退出。
                    RequireConsent = false,
                    AllowAccessToAllScopes = true,
                    LogoutUri = host + "account/logout-callback",
                    LogoutSessionRequired = true
                };
            }
        }

        public static Client JSClient
        {
            get
            {
                var host = "http://client.js.dev";
                return new Client
                {
                    Enabled = true,
                    ClientName = "JS Client",
                    ClientId = "js-client",
                    Flow = Flows.Implicit,
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
                    AllowAccessToAllScopes = true,
                    AccessTokenLifetime = 10
                };
            }
        }

        public static Client OAuth2AuthorizationCodeFlowClientForUWP
        {
            get
            {
                var host = "http://uwp.oauth2-authorization-code-flow.dev";

                return new Client
                {
                    Enabled = true,
                    ClientName = "OAuth2 Authorization Code Flow Client",
                    ClientId = "oauth2-authorization-code-flow.uwp",
                    Flow = Flows.AuthorizationCode,
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
                    AllowAccessToAllScopes = true
                };
            }
        }
    }
}