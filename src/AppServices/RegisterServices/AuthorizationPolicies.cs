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
        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(nameof(Policies.ActiveUser), Policies.ActiveUser());
            opts.AddPolicy(nameof(Policies.AdminUser), Policies.AdminUser());
            opts.AddPolicy(nameof(Policies.LoggedIn), Policies.LoggedIn());
            opts.AddPolicy(nameof(Policies.SiteMaintainer), Policies.SiteMaintainer());
            opts.AddPolicy(nameof(Policies.StaffUser), Policies.StaffUser());
            opts.AddPolicy(nameof(Policies.UserAdministrator), Policies.UserAdministrator());
        });

        services.AddSingleton<IAuthorizationHandler>(_ => new ActionItemUpdatePermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new CaseworkUpdatePermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new CaseworkViewPermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new ContactUpdatePermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new CustomerUpdatePermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new CustomerViewPermissionsHandler());
    }
}
