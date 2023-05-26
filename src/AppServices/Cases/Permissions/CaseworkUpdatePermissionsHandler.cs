using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.Cases.Permissions;

internal class CaseworkUpdatePermissionsHandler :
    AuthorizationHandler<CaseworkOperation, CaseworkUpdateDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CaseworkOperation requirement,
        CaseworkUpdateDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CaseworkOperation.Edit) =>
                // Cases can only be edited if they and the associated Customer are not deleted.
                IsStaffUser(context.User) && IsNotDeleted(resource) && CustomerIsNotDeleted(resource),

            nameof(CaseworkOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                IsAdminUser(context.User),

            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsAdminUser(IPrincipal user) => user.IsInRole(RoleName.Admin);

    private static bool IsStaffUser(IPrincipal user) => user.IsInRole(RoleName.Staff) || IsAdminUser(user);

    private static bool IsNotDeleted(CaseworkUpdateDto resource) => !resource.IsDeleted;

    private static bool CustomerIsNotDeleted(CaseworkUpdateDto resource) => !resource.CustomerIsDeleted;
}
