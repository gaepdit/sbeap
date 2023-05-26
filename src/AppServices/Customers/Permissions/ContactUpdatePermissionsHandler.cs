using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.Customers.Permissions;

internal class ContactUpdatePermissionsHandler :
    AuthorizationHandler<CustomerOperation, ContactUpdateDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CustomerOperation requirement,
        ContactUpdateDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CustomerOperation.Edit) =>
                // Contacts can only be edited if they and the associated Customer are not deleted.
                IsStaffUser(context.User) && CustomerIsNotDeleted(resource),

            nameof(CustomerOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                IsAdminUser(context.User),

            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsAdminUser(IPrincipal user) => user.IsInRole(RoleName.Admin);

    private static bool IsStaffUser(IPrincipal user) => user.IsInRole(RoleName.Staff) || IsAdminUser(user);

    private static bool CustomerIsNotDeleted(ContactUpdateDto resource) => !resource.CustomerIsDeleted;
}
