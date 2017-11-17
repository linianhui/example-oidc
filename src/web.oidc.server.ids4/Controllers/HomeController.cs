using Microsoft.AspNetCore.Mvc;

namespace ServerSite.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Content("<a href='/.well-known/openid-configuration'>ids4 discovery api</a>", "text/html");
        }
    }
}