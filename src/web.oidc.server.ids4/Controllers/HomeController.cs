using Microsoft.AspNetCore.Mvc;

namespace ServerSite.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Content("<h1>ids4 ok</h1>");
        }
    }
}