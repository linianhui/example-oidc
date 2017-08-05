using System.Web.Http;

namespace OAuth2.Resources.Apis
{
    [RoutePrefix("")]
    public class IndexApiController : ApiController
    {
        [HttpGet]
        [Route("")]
        public object GetValues()
        {
            return new
            {
                text = "this is a api.",
                values = this.Url.Link("get-values", null)
            };
        }
    }
}