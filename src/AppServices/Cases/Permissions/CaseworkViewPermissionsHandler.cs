using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.Domain.Identity;
using System.Security.Principal;

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
                IsStaffUser(context.User) && IsNotDeleted(resource),

            nameof(CaseworkOperation.EditActionItems) =>
                // Action Items can only be edited if the Case is still open.
                IsStaffUser(context.User) && IsOpen(resource) && IsNotDeleted(resource),

            nameof(CaseworkOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                IsAdminUser(context.User) && CustomerIsNotDeleted(resource),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsAdminUser(IPrincipal user) => user.IsInRole(RoleName.Admin);
    private static bool IsStaffUser(IPrincipal user) => user.IsInRole(RoleName.Staff) || user.IsInRole(RoleName.Admin);
    private static bool IsNotDeleted(CaseworkViewDto resource) => !resource.IsDeleted && CustomerIsNotDeleted(resource);
    private static bool CustomerIsNotDeleted(CaseworkViewDto resource) => !resource.Customer.IsDeleted;
    private static bool IsOpen(CaseworkViewDto resource) => resource.CaseClosedDate is null;
}
