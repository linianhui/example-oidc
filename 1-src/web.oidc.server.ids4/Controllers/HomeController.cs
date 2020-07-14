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
                    ["client"] = this.Url.Link("debug.client.get", null),
                    ["user"] = this.Url.Link("debug.user.get", null),
                    ["user_claim"] = this.Url.Link("debug.user_claim.get", null),
                    ["api_scope"] = this.Url.Link("debug.api_resource.get", null),
                    ["identity_resource"] = this.Url.Link("debug.identity_resource.get", null)
                }
            });
        }
    }
}
