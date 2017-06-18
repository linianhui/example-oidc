using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Core.OAuth2.Host.Controllers
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