using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace OAuth2.Github.AspNetCore
{
    internal class GithubOAuthHandler : OAuthHandler<GithubOAuthOptions>
    {
        public GithubOAuthHandler(
            IOptionsMonitor<GithubOAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity,
            AuthenticationProperties properties, 
            OAuthTokenResponse tokens)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, base.Options.UserInformationEndpoint);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            var httpResponseMessage = await base.Backchannel.SendAsync(httpRequestMessage, base.Context.RequestAborted);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Github user information ({httpResponseMessage.StatusCode}).");
            }
            var user = JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync());
            var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, base.Context, base.Scheme, base.Options, base.Backchannel, tokens, user);
            context.RunClaimActions();
            await base.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, base.Scheme.Name);
        }
    }
}