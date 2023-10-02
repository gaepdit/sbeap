using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Permissions.Helpers;

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
                context.User.IsStaff() && IsNotDeleted(resource),

            nameof(CaseworkOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                context.User.IsAdmin(),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsNotDeleted(ActionItemUpdateDto resource) =>
        resource is { IsDeleted: false, CaseworkIsDeleted: false };
}
