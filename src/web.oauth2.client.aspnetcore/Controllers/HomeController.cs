using Microsoft.AspNetCore.Mvc;

namespace ClientSite.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(User?.Claims);
        }
    }
}