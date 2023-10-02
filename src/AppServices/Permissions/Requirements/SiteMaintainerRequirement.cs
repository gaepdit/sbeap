using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Permissions.Helpers;

namespace Sbeap.AppServices.Permissions.Requirements;

internal class SiteMaintainerRequirement :
    AuthorizationHandler<SiteMaintainerRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        SiteMaintainerRequirement requirement)
    {
        if (context.User.IsSiteMaintainer())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
