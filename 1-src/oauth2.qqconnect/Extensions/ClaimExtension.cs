using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OAuth2.QQConnect.Extensions
{
    public static class ClaimExtension
    {
        public static string FindFirstValue(this ICollection<Claim> @this, string type)
        {
            return @this?.FirstOrDefault(claim => claim.Type == type)?.Value;
        }
    }
}
