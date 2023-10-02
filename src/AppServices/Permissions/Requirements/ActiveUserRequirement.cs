using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Permissions.Helpers;

namespace Sbeap.AppServices.Permissions.Requirements;

internal class ActiveUserRequirement :
    AuthorizationHandler<ActiveUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        if (context.User.IsActive())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
