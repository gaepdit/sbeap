using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Permissions.Helpers;

namespace Sbeap.AppServices.Permissions.Requirements;

internal class AdministrationViewRequirement :
    AuthorizationHandler<AdministrationViewRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdministrationViewRequirement requirement)
    {
        if (context.User.IsStaffOrMaintainer())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
