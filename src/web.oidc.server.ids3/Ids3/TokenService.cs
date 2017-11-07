using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;

namespace ServerSite.Ids3
{
    public class TokenService : DefaultTokenService
    {
        public TokenService(
            IdentityServerOptions options,
            IClaimsProvider claimsProvider,
            ITokenHandleStore tokenHandles,
            ITokenSigningService signingService,
            IEventService events)
            : base(options, claimsProvider, tokenHandles, signingService, events)
        {
        }

        public TokenService(
            IdentityServerOptions options,
            IClaimsProvider claimsProvider,
            ITokenHandleStore tokenHandles,
            ITokenSigningService signingService,
            IEventService events,
            OwinEnvironmentService owinEnvironmentService)
            : base(options, claimsProvider, tokenHandles, signingService, events, owinEnvironmentService)
        {
        }

        public override async Task<Token> CreateAccessTokenAsync(TokenCreationRequest request)
        {
            var token = await base.CreateAccessTokenAsync(request);
            var sessionId = request.ValidatedRequest.SessionId;
            if (string.IsNullOrWhiteSpace(sessionId) == false)
            {
                token.Claims.Add(new Claim(Constants.ClaimTypes.SessionId, sessionId.Trim()));
            }
            return token;
        }
    }
}