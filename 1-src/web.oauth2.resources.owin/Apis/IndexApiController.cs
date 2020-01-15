using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Web.OAuth2.Resources.Apis
{
    [Authorize]
    [RoutePrefix("")]
    public class IndexApiController : ApiController
    {
        [HttpGet]
        [Route("")]
        public object GetValues()
        {
            var claims = ((ClaimsPrincipal)this.User).Claims
               .GroupBy(_ => _.Type)
               .Select(_ => new KeyValuePair<string, string[]>(_.Key, _.Select(c => c.Value).ToArray()))
               .ToDictionary(_ => _.Key, _ => _.Value);

            return new
            {
                _claims = claims,
                text = "this is a asp net web api."
            };
        }
    }
}