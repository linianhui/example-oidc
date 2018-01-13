using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Filters.Permissions
{
    public class ForbiddenResult : JsonResult
    {
        public ForbiddenResult(string permissionName) : base(null)
        {
            base.StatusCode = StatusCodes.Status403Forbidden;
            base.Value = new
            {
                error = "no_permission",
                message = $"缺失{permissionName}权限"
            };
        }
    }
}