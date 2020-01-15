using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ServerSite.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            string authority = new Uri(this.Request.GetDisplayUrl())
                .GetLeftPart(UriPartial.Authority);

            return Json(new Dictionary<string, object>
            {
                ["oidc_discovery_url"] = authority + "/.well-known/openid-configuration",
                ["debug"] = new Dictionary<string, string>
                {
                    ["clients"] = this.Url.Link("debug.clients.get", null),
                    ["users"] = this.Url.Link("debug.users.get", null),
                    ["user_claims"] = this.Url.Link("debug.user_claims.get", null),
                    ["api_resources"] = this.Url.Link("debug.api_resources.get", null),
                    ["identity_resources"] = this.Url.Link("debug.identity_resources.get", null)
                }
            });
        }
    }
}