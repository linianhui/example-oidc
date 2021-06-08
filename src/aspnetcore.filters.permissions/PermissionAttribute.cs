using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Filters.Permissions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        public string Name { get; set; }

        public PermissionAttribute(string name)
        {
            Name = name;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissionHandlerProvider = context.HttpContext.RequestServices.GetRequiredService<IPermissionHandlerProvider>();
            var permissionHandler =  permissionHandlerProvider.GetHandler(context);
            var hasPermission =  permissionHandler.HasPermission(context.HttpContext.User, this.Name);
            if (hasPermission == false)
            {
                context.Result = new ForbiddenResult(Name);
            }
        }
    }
}
