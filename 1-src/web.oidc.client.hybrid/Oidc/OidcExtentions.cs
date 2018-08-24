using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ClientSite.Oidc
{
    public static class OidcExtentions
    {
        public static void AddOidcAuthentication(this IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(_ =>
            {
                _.DefaultScheme = Constants.AuthenticationSchemeOfCookies;
                _.DefaultChallengeScheme = Constants.AuthenticationSchemeOfOidc;
            })
            .AddCookie(Constants.AuthenticationSchemeOfCookies, _ =>
            {
                _.Cookie.Name = Constants.CookieName;
            })
            .AddOpenIdConnect(Constants.AuthenticationSchemeOfOidc, _ =>
            {
                _.SignInScheme = Constants.AuthenticationSchemeOfCookies;
                _.Authority = "http://oidc-server.test";
                _.RequireHttpsMetadata = false;
                _.ClientId = "oidc-client-hybrid.test";
                _.ClientSecret = "lnh";
                _.ResponseType = "code id_token";
                _.SaveTokens = true;
                _.GetClaimsFromUserInfoEndpoint = false;
                _.CallbackPath = new PathString("/oidc/login-callback");
                _.SignOutScheme = Constants.AuthenticationSchemeOfCookies;
                _.RemoteSignOutPath = new PathString("/oidc/front-channel-logout-callback");
                _.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = OnRedirectToIdentityProvider,
                    OnRemoteSignOut = OnRemoteSignOut,
                    OnRemoteFailure = OnRemoteFailure,
                    OnAuthenticationFailed = OnAuthenticationFailed,
                    OnRedirectToIdentityProviderForSignOut = OnRedirectToIdentityProviderForSignOut,
                    OnAuthorizationCodeReceived = OnAuthorizationCodeReceived,
                    OnMessageReceived = OnMessageReceived,
                    OnTicketReceived = OnTicketReceived,
                    OnTokenResponseReceived = OnTokenResponseReceived,
                    OnTokenValidated = OnTokenValidated,
                    OnUserInformationReceived = OnUserInformationReceived
                };
            });
        }

        private static Task OnRedirectToIdentityProvider(RedirectContext context)
        {
            if (context.HttpContext.Items.ContainsKey("idp"))
            {
                var idp = context.HttpContext.Items["idp"];
                context.ProtocolMessage.AcrValues = "idp:" + idp;
            }

            return Task.FromResult(0);
        }

        private static Task OnRemoteSignOut(RemoteSignOutContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnRemoteFailure(RemoteFailureContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnRedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            context.ProtocolMessage.PostLogoutRedirectUri = context.Request.Scheme + "://" + context.Request.Host;
            return Task.FromResult(0);
        }
        private static Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            return Task.FromResult(0);
        }
        private static Task OnMessageReceived(MessageReceivedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnTicketReceived(TicketReceivedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnTokenResponseReceived(TokenResponseReceivedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnTokenValidated(TokenValidatedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnUserInformationReceived(UserInformationReceivedContext context)
        {
            return Task.FromResult(0);
        }

    }
}