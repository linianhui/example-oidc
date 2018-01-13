using System.Security.Claims;

namespace AspNetCore.Filters.Permissions
{
    public interface IPermissionHandler
    {
        bool HasPermission(ClaimsPrincipal principal, string permissionName);
    }
}