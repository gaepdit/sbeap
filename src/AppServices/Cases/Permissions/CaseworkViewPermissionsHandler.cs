using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Permissions.Helpers;

namespace Sbeap.AppServices.Cases.Permissions;

internal class CaseworkViewPermissionsHandler :
    AuthorizationHandler<CaseworkOperation, CaseworkViewDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CaseworkOperation requirement,
        CaseworkViewDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CaseworkOperation.Edit) =>
                // Cases can only be edited if they and the associated Customer are not deleted.
                context.User.IsStaff() && IsNotDeleted(resource),

            nameof(CaseworkOperation.EditActionItems) =>
                // Action Items can only be edited if the Case is still open.
                context.User.IsStaff() && IsOpen(resource) && IsNotDeleted(resource),

            nameof(CaseworkOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                context.User.IsAdmin() && CustomerIsNotDeleted(resource),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsNotDeleted(CaseworkViewDto resource) => !resource.IsDeleted && CustomerIsNotDeleted(resource);
    private static bool CustomerIsNotDeleted(CaseworkViewDto resource) => !resource.Customer.IsDeleted;
    private static bool IsOpen(CaseworkViewDto resource) => resource.CaseClosedDate is null;
}
