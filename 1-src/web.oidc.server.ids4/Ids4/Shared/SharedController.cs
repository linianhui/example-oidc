using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;

namespace ServerSite.Ids4.Shared
{
    public class SharedController : Controller
    {
        private readonly IIdentityServerInteractionService _idsInteraction;

        public SharedController(IIdentityServerInteractionService idsInteraction)
        {
            _idsInteraction = idsInteraction;
        }

        [HttpGet]
        [Route("ids4/error")]
        public async Task<IActionResult> Error(string errorId)
        {
            var error = await _idsInteraction.GetErrorContextAsync(errorId);
            return View(error);
        }
    }
}
