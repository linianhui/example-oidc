using System.Security.Claims;
using System.Web.Mvc;

namespace Owin.OAuth2.Host.Controllers
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