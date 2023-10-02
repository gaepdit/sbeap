using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Permissions.Helpers;

namespace Sbeap.AppServices.Permissions.Requirements;

internal class StaffUserRequirement :
    AuthorizationHandler<StaffUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        StaffUserRequirement requirement)
    {
        if (context.User.IsStaff())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
