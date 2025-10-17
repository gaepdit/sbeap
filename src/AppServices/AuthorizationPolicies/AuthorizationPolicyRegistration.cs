using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.AppServices.Customers.Permissions;
using System.Diagnostics.CodeAnalysis;

namespace Sbeap.AppServices.AuthorizationPolicies;

[SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out")]
public static class AuthorizationPolicyRegistration
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddPolicies();

        // Resource/operation-based permission handlers, e.g.:
        // var canAssign = await authorization.Succeeded(User, entryView, WorkEntryOperation.EditWorkEntry);

        // ViewRequirements are added as scoped if they consume scoped services; otherwise as singletons.
        services
            .AddSingleton<IAuthorizationHandler, ActionItemUpdatePermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, CaseworkUpdatePermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, CaseworkViewPermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, ContactUpdatePermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, CustomerUpdatePermissionsHandler>()
            .AddSingleton<IAuthorizationHandler, CustomerViewPermissionsHandler>();

        return services;
    }
}
