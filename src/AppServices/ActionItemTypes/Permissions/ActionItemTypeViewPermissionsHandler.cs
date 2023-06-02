using Microsoft.AspNetCore.Authorization;
using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.ActionItemTypes.Permissions;

internal class ActionItemTypeViewPermissionsHandler :
    AuthorizationHandler<ActionItemTypeOperation, ActionItemTypeViewDto>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionItemTypeOperation requirement, ActionItemTypeViewDto resource)
    {
        throw new NotImplementedException();
    }

    private static bool IsAdminUser(IPrincipal user) => user.IsInRole(RoleName.Admin);

    private static bool IsStaffUser(IPrincipal user) => user.IsInRole(RoleName.Staff) || user.IsInRole(RoleName.Admin);

    private static bool IsNotDeleted(ActionItemTypeViewDto resource) => resource.Active;
}
