using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.ActionItemTypes.Permissions;
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
            opts.AddPolicy(PolicyName.AdminUser, Policies.AdminUserPolicy());
            opts.AddPolicy(PolicyName.LoggedIn, Policies.LoggedInPolicy());
            opts.AddPolicy(PolicyName.SiteMaintainer, Policies.SiteMaintainerPolicy());
            opts.AddPolicy(PolicyName.StaffUser, Policies.StaffUserPolicy());
            opts.AddPolicy(PolicyName.UserAdministrator, Policies.UserAdministratorPolicy());
        });

        services.AddSingleton<IAuthorizationHandler>(_ => new CaseworkViewPermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new CaseworkUpdatePermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new ContactUpdatePermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new CustomerViewPermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new CustomerUpdatePermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new ActionItemTypeUpdatePermissionsHandler());
        services.AddSingleton<IAuthorizationHandler>(_ => new ActionItemTypeViewPermissionsHandler());
    }
}
