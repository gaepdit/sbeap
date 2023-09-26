using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Permissions.Helpers;

namespace Sbeap.AppServices.Permissions.Requirements;

internal class AdminUserRequirement :
    AuthorizationHandler<AdminUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdminUserRequirement requirement)
    {
        if (context.User.IsAdmin())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
