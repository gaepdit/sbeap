using Microsoft.AspNetCore.Authorization;
using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.Agencies.Permissions;

internal class AgencyViewPermissionsHandler : AuthorizationHandler<AgencyOperation, AgencyViewDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AgencyOperation requirement,
        AgencyViewDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        // Differentiating Editing and Delete/Restore even though both are presently Admin only
        // in case staff users will need rights to these in the future
        bool success = requirement.Name switch
        {
            nameof(AgencyOperation.Edit) =>
                // Only an Admin User can edit.
                IsAdminUser(context.User),

            nameof(AgencyOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                IsAdminUser(context.User),

            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsAdminUser(IPrincipal user) => user.IsInRole(RoleName.Admin);
}
