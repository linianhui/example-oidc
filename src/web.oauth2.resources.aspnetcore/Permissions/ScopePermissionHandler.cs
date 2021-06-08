using System.Security.Claims;
using AspNetCore.Filters.Permissions;

namespace Web.OAuth2.Resources.Permissions
{
    public class ScopePermissionHandler : IPermissionHandler
    {
        public bool HasPermission(ClaimsPrincipal principal, string permissionName)
        {
            //todo:自定义部分，通过Scope获取到到其拥有那些Permission
            return false;
        }
    }
}
