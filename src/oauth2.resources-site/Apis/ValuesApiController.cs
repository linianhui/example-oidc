using System.Web.Http;

namespace OAuth2.Resources.Apis
{
    [RoutePrefix("values")]
    public class ValuesApiController : ApiController
    {
        [HttpGet]
        [Route("", Name = "get-values")]
        public string[] GetValues()
        {
            return new[] { "value1", "value2", "lnhcode@outlook.com" };
        }
    }
}