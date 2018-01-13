using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCore.Filters.Permissions
{
    public interface IPermissionHandlerProvider
    {
        IPermissionHandler GetHandler(AuthorizationFilterContext context);
    }
}