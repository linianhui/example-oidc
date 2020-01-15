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

        [HttpGet("clients", Name = "debug.clients.get")]
        public IEnumerable<Client> GetClients()
        {
            return Clients.All;
        }

        [HttpGet("users", Name = "debug.users.get")]
        public IEnumerable<TestUser> GetUsers()
        {
            return Users.All;
        }

        [HttpGet("user-claims", Name = "debug.user_claims.get")]
        public JsonResult GetUser()
        {
            return Json(base.User?.Claims.Select(_ => new
            {
                type = _.Type,
                value = _.Value
            }));
        }

        [HttpGet("api-resources", Name = "debug.api_resources.get")]
        public IEnumerable<ApiResource> GetApiResources()
        {
            return Ids4.Resources.AllApiResources;
        }

        [HttpGet("identity-resources", Name = "debug.identity_resources.get")]
        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return Ids4.Resources.AllIdentityResources;
        }
    }
}