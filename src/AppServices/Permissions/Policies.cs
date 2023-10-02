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
    // Default policy builders
    private static AuthorizationPolicyBuilder AuthenticatedUserPolicyBuilder =>
        new AuthorizationPolicyBuilder().RequireAuthenticatedUser();

    private static AuthorizationPolicyBuilder ActiveUserPolicyBuilder =>
        AuthenticatedUserPolicyBuilder.AddRequirements(new ActiveUserRequirement());

    // Basic policies
    public static AuthorizationPolicy ActiveUser => ActiveUserPolicyBuilder.Build();

    public static AuthorizationPolicy LoggedInUser => AuthenticatedUserPolicyBuilder.Build();

    // Role-based policies
    public static AuthorizationPolicy AdministrationView =>
        ActiveUserPolicyBuilder.AddRequirements(new AdministrationViewRequirement()).Build();

    public static AuthorizationPolicy AdminUser =>
        ActiveUserPolicyBuilder.AddRequirements(new AdminUserRequirement()).Build();

    public static AuthorizationPolicy SiteMaintainer =>
        ActiveUserPolicyBuilder.AddRequirements(new SiteMaintainerRequirement()).Build();

    public static AuthorizationPolicy StaffUser =>
        ActiveUserPolicyBuilder.AddRequirements(new StaffUserRequirement()).Build();

    public static AuthorizationPolicy UserAdministrator =>
        ActiveUserPolicyBuilder.AddRequirements(new UserAdminRequirement()).Build();
}
