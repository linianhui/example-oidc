using System.Security.Claims;
using System.Web.Mvc;

namespace Client.Implicit.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            return View((User as ClaimsPrincipal)?.Claims);
        }
    }
}