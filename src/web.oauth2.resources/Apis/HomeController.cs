using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Web.OAuth2.Resources.Apis
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public object Index()
        {
            var claims = this.User.Claims
                .GroupBy(_ => _.Type)
                .Select(_ => new KeyValuePair<string, string[]>(_.Key, _.Select(c => c.Value).ToArray()))
                .ToDictionary(_ => _.Key, _ => _.Value);

            return new
            {
                _claims = claims,
                author = "lnhcode@outlook.com",
                github = "https://github.com/linianhui/oidc.example",
                text = "this is a api.",
                values = this.Url.Link("books", null)
            };
        }
    }
}