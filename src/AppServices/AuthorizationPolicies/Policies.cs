using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.AuthenticationServices;
using Sbeap.AppServices.AuthenticationServices.Claims;

namespace Sbeap.AppServices.AuthorizationPolicies;

#pragma warning disable S125 // Sections of code should not be commented out
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
    public static void AddPolicies(this IServiceCollection services) => services.AddAuthorizationBuilder()
        .AddPolicy(nameof(ActiveUser), ActiveUser)
        .AddPolicy(nameof(AdministrationView), AdministrationView)
        .AddPolicy(nameof(AdminUser), AdminUser)
        .AddPolicy(nameof(SiteMaintainer), SiteMaintainer)
        .AddPolicy(nameof(StaffUser), StaffUser)
        .AddPolicy(nameof(UserAdministrator), UserAdministrator);

    // Default policy builder
    private static AuthorizationPolicyBuilder ActiveUserPolicyBuilder => new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim(AppClaimTypes.ActiveUser, true.ToString());

    // Claims-based policies
    public static AuthorizationPolicy ActiveUser { get; } = ActiveUserPolicyBuilder.Build();

    // Role-based policies
    public static AuthorizationPolicy AdministrationView { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsStaffOrMaintainer()).Build();

    public static AuthorizationPolicy AdminUser { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsAdmin()).Build();

    public static AuthorizationPolicy SiteMaintainer { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsSiteMaintainer()).Build();

    public static AuthorizationPolicy StaffUser { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsStaff()).Build();

    public static AuthorizationPolicy UserAdministrator { get; } = ActiveUserPolicyBuilder
        .RequireAssertion(context => context.User.IsUserAdmin()).Build();
}
