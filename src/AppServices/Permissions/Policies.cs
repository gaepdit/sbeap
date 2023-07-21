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

public static class Policies
{
    public static AuthorizationPolicy ActiveUser() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new ActiveUserRequirement())
            .Build();

    public static AuthorizationPolicy AdminUser() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new ActiveUserRequirement())
            .AddRequirements(new AdminUserRequirement())
            .Build();

    public static AuthorizationPolicy LoggedIn() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

    public static AuthorizationPolicy SiteMaintainer() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new ActiveUserRequirement())
            .AddRequirements(new SiteMaintainerRequirement())
            .Build();

    public static AuthorizationPolicy StaffUser() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new ActiveUserRequirement())
            .AddRequirements(new StaffUserRequirement())
            .Build();

    public static AuthorizationPolicy UserAdministrator() =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new ActiveUserRequirement())
            .AddRequirements(new UserAdministratorRequirement())
            .Build();
}
