using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Permissions.Requirements;

namespace Sbeap.AppServices.Permissions;

#pragma warning disable S125
//
// Two ways to use these policies:
//
// A. As an attribute on a PageModel class:
//
//    [Authorize(Policy = PolicyName.SiteMaintainer)]
//    public class AddModel : PageModel
//
// B. From a DI authorization service: 
//
//    public async Task<IActionResult> OnGetAsync([FromServices] IAuthorizationService authorizationService)
//    {
//        var isStaff = (await authorizationService.AuthorizeAsync(User, Policies.StaffUserPolicy())).Succeeded;
//    }
//
#pragma warning restore S125

public static class PolicyName
{
    public const string AdminUser = nameof(AdminUser);
    public const string StaffUser = nameof(StaffUser);
    public const string SiteMaintainer = nameof(SiteMaintainer);
    public const string UserAdministrator = nameof(UserAdministrator);
}

public static class Policies
{
    public static AuthorizationPolicy AdminUserPolicy() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new AdminUserRequirement())
            .Build();

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
