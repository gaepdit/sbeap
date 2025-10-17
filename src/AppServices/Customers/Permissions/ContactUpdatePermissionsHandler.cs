using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.AuthenticationServices;
using Sbeap.AppServices.Customers.Dto;

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
                context.User.IsStaff() && IsNotDeleted(resource),

            nameof(CustomerOperation.ManageDeletions) =>
                // Only an Admin User can delete or restore.
                context.User.IsAdmin(),

            _ => false,
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }

    private static bool IsNotDeleted(ContactUpdateDto resource) =>
        resource is { IsDeleted: false, CustomerIsDeleted: false };
}
