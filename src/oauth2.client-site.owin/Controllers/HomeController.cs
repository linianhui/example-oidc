using System.Security.Claims;
using System.Web.Mvc;

namespace ClientSite.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View((User as ClaimsPrincipal)?.Claims);
        }
    }
}