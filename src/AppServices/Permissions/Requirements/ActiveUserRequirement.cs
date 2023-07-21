using Microsoft.AspNetCore.Authorization;

namespace Sbeap.AppServices.Permissions.Requirements;

internal class ActiveUserRequirement :
    AuthorizationHandler<ActiveUserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        if (context.User.HasClaim(c => c is { Type: "ActiveUser", Value: "True" }))
            context.Succeed(requirement);

        return Task.FromResult(0);
    }
}
