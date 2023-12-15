using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.AppServices.Customers.Permissions;
using Sbeap.AppServices.Permissions;

namespace Sbeap.AppServices.RegisterServices;

public static class AuthorizationPolicies
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        // Authorization policies
        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(Policies.ActiveUser), Policies.ActiveUser)
            .AddPolicy(nameof(Policies.AdministrationView), Policies.AdministrationView)
            .AddPolicy(nameof(Policies.AdminUser), Policies.AdminUser)
            .AddPolicy(nameof(Policies.LoggedInUser), Policies.LoggedInUser)
            .AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer)
            .AddPolicy(nameof(Policies.StaffUser), Policies.StaffUser)
            .AddPolicy(nameof(Policies.UserAdministrator), Policies.UserAdministrator);

        // Resource-based handlers
        services.AddSingleton<IAuthorizationHandler, ActionItemUpdatePermissionsHandler>();
        services.AddSingleton<IAuthorizationHandler, CaseworkUpdatePermissionsHandler>();
        services.AddSingleton<IAuthorizationHandler, CaseworkViewPermissionsHandler>();
        services.AddSingleton<IAuthorizationHandler, ContactUpdatePermissionsHandler>();
        services.AddSingleton<IAuthorizationHandler, CustomerUpdatePermissionsHandler>();
        services.AddSingleton<IAuthorizationHandler, CustomerViewPermissionsHandler>();
    }
}
