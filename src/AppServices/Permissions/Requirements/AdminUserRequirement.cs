using Microsoft.AspNetCore.Authorization;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.Permissions.Requirements;

internal class AdminUserRequirement :
    AuthorizationHandler<AdminUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdminUserRequirement requirement)
    {
        if (context.User.IsInRole(RoleName.Admin))
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
