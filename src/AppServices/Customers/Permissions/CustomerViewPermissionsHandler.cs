using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Identity;
using System.Security.Principal;

namespace Sbeap.AppServices.Customers.Permissions;

internal class CustomerViewPermissionsHandler :
    AuthorizationHandler<CustomerOperation, CustomerViewDto>
{
    private static bool IsAdminUser(IPrincipal user) =>
        user.IsInRole(RoleName.Admin);

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CustomerOperation requirement,
        CustomerViewDto resource)
    {
        if (!(context.User.Identity?.IsAuthenticated ?? false))
            return Task.FromResult(0);

        var success = requirement.Name switch
        {
            CustomerOperationNames.ManageDeletions =>
                // Only an Admin User can delete or restore.
                IsAdminUser(context.User),

            _ => throw new ArgumentOutOfRangeException(nameof(requirement)),
        };

        if (success) context.Succeed(requirement);
        return Task.FromResult(0);
    }
}
