using Microsoft.AspNetCore.Authorization;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.Permissions.Requirements;

internal class StaffUserRequirement :
    AuthorizationHandler<StaffUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        StaffUserRequirement requirement)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        if (context.User.IsInRole(RoleName.Staff))
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
