using AspNetCore.Filters.Permissions;
using Microsoft.Extensions.DependencyInjection;

namespace Web.OAuth2.Resources.Permissions
{
    public static class PermissionExtension
    {
        public static void AddPermissions(this IServiceCollection services)
        {
            services.AddScoped<ScopePermissionHandler>();
            services.AddScoped<RolePermissionHandler>();
            services.AddSingleton<IPermissionHandlerProvider, PermissionHandlerProvider>();
        }
    }
}
