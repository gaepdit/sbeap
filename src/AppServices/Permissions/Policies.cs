using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Permissions.Requirements;

namespace Sbeap.AppServices.Permissions;

public static class PolicyName
{
    public const string StaffUser = nameof(StaffUser);
    public const string SiteMaintainer = nameof(SiteMaintainer);
    public const string UserAdministrator = nameof(UserAdministrator);
}

public static class Policies
{
    public static AuthorizationPolicy StaffUserPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new StaffUserRequirement())
            .Build();

    public static AuthorizationPolicy SiteMaintainerPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new SiteMaintainerRequirement())
            .Build();

    public static AuthorizationPolicy UserAdministratorPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new UserAdministratorRequirement())
            .Build();
}
