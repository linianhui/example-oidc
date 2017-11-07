using System.Web.Mvc;
using IdentityServer3.Core;
using ServerSite.Ids3;

namespace ServerSite.Controllers
{
    [AllowAnonymous]
    [TryAuthentication(Constants.PrimaryAuthenticationType)]
    [RoutePrefix(Ids3Constants.ServerPath)]
    public class AuthController : Controller
    {
        [HttpGet]
        [Route("js/account")]
        public JavaScriptResult IsLogin()
        {
            var isLogin = base.User?.Identity?.IsAuthenticated ?? false;
            var javaScript = "var account={"
                           + "is_login:" + isLogin.ToString().ToLower()
                           + "};";
            return JavaScript(javaScript);
        }
    }
}