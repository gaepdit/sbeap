using Microsoft.AspNetCore.Authorization;
using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.ActionItemTypes.Permissions;

internal class ActionItemTypeUpdatePermissionsHandler :
    AuthorizationHandler<ActionItemTypeOperation, ActionItemTypeUpdateDto>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActionItemTypeOperation requirement, ActionItemTypeUpdateDto resource)
    {
        throw new NotImplementedException();
    }

    private static bool IsAdminUser(IPrincipal user) => user.IsInRole(RoleName.Admin);

    private static bool IsStaffUser(IPrincipal user) => user.IsInRole(RoleName.Staff) || IsAdminUser(user);

    private static bool IsNotDeleted(ActionItemTypeUpdateDto resource) => resource.Active;
}
