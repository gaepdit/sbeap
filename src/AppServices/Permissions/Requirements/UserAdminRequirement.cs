using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Permissions.Helpers;

namespace Sbeap.AppServices.Permissions.Requirements;

internal class UserAdminRequirement :
    AuthorizationHandler<UserAdminRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserAdminRequirement requirement)
    {
        if (context.User.IsUserAdmin())
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
