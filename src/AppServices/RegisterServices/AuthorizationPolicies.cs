using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.Permissions;

namespace Sbeap.AppServices.RegisterServices;

public static class AuthorizationPolicies
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(PolicyName.SiteMaintainer, Policies.SiteMaintainerPolicy());
            opts.AddPolicy(PolicyName.UserAdministrator, Policies.UserAdministratorPolicy());
        });
    }
}
