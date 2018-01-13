using System.Security.Claims;
using AspNetCore.Filters.Permissions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Web.OAuth2.Resources.Permissions
{
    public class PermissionHandlerProvider : IPermissionHandlerProvider
    {
        public IPermissionHandler GetHandler(AuthorizationFilterContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            //如果认证后的信息中含有用户Id,则采用基于Role的方式管理权限验证
            //当前授权是针对用户的，则一般是：用户拥有多个角色，一个角色包含一组Permission。
            var subject = context.HttpContext.User.FindFirstValue("sub");
            if (subject != null)
            {
                return serviceProvider.GetRequiredService<RolePermissionHandler>();
            }

            //采用基于Scope的方式管理权限验证
            //比如OAuth2中的4种授权流程，Token的受众皆不是直接的用户，而是一个客户端。
            //这种情况下则是都是通过Scope来管理权限的：比如一个客户端被授予了多个Scope，每一个Scope包含一组Permission。
            return serviceProvider.GetRequiredService<ScopePermissionHandler>();
        }
    }
}
