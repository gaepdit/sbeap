using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.Cases.Permissions;

internal class ActionItemUpdatePermissionsHandler :
    AuthorizationHandler<CaseworkOperation, ActionItemUpdateDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CaseworkOperation requirement,
        ActionItemUpdateDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CaseworkOperation.EditActionItems) =>
                // Action Items can only be edited if they and the associated Case are not deleted.
                IsStaffUser(context.User) && IsNotDeleted(resource),

            nameof(CaseworkOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                IsAdminUser(context.User),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsAdminUser(IPrincipal user) => user.IsInRole(RoleName.Admin);
    private static bool IsStaffUser(IPrincipal user) => user.IsInRole(RoleName.Staff) || IsAdminUser(user);

    private static bool IsNotDeleted(ActionItemUpdateDto resource) =>
        resource is { IsDeleted: false, CaseIsDeleted: false };
}
