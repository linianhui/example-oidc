using System.IdentityModel.Tokens;
using System.Web.Http;

namespace OAuth2.Resources.Apis
{
    [Authorize]
    [RoutePrefix("values")]
    public class ValuesApiController : ApiController
    {
        [HttpGet]
        [Route("", Name = "get-values")]
        public object GetValues()
        {
            var tokenString = this.Request.Headers.Authorization.Parameter;
            var jwt = new JwtSecurityToken(tokenString);

            return new
            {
                _self = this.Request.RequestUri,
                _jwt_token = new
                {
                    header = jwt.Header,
                    payload = jwt.Payload
                },
                author = "lnhcode@outlook.com",
                github = "https://github.com/linianhui/oidc.example"
            };
        }
    }
}