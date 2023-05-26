using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.Customers.Permissions;

internal class CustomerUpdatePermissionsHandler :
    AuthorizationHandler<CustomerOperation, CustomerUpdateDto>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CustomerOperation requirement,
        CustomerUpdateDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            nameof(CustomerOperation.Edit) =>
                // Customers can only be edited if they are not deleted.
                IsStaffUser(context.User) && IsNotDeleted(resource),

            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsStaffUser(IPrincipal user) => user.IsInRole(RoleName.Staff) || user.IsInRole(RoleName.Admin);

    private static bool IsNotDeleted(CustomerUpdateDto resource) => !resource.IsDeleted;
}
