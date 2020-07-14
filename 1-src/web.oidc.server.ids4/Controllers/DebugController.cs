using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Mvc;
using ServerSite.Ids4;
using System.Collections.Generic;
using System.Linq;

namespace ServerSite.Controllers
{
    [Route(".debug")]
    [ApiController]
    public class DebugController : Controller
    {

        [HttpGet("client", Name = "debug.client.get")]
        public IEnumerable<Client> GetClients()
        {
            return Clients.All;
        }

        [HttpGet("user", Name = "debug.user.get")]
        public IEnumerable<TestUser> GetUsers()
        {
            return Users.All;
        }

        [HttpGet("user-claim", Name = "debug.user_claim.get")]
        public JsonResult GetUser()
        {
            return Json(base.User?.Claims.Select(_ => new
            {
                type = _.Type,
                value = _.Value
            }));
        }

        [HttpGet("api-scope", Name = "debug.api_scope.get")]
        public IEnumerable<ApiScope> GetApiResources()
        {
            return Ids4.Resources.AllApiScopes;
        }

        [HttpGet("identity-resource", Name = "debug.identity_resource.get")]
        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return Ids4.Resources.AllIdentityResources;
        }
    }
}
