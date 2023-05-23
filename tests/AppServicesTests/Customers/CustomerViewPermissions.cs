using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Permissions;
using Sbeap.Domain.Identity;
using System.Security.Claims;

namespace AppServicesTests.Customers;

public class CustomerViewPermissions
{
    private readonly CustomerOperation[] _requirements = { CustomerOperation.ManageDeletions };

    private static CustomerViewDto EmptyCustomerView => new();

    [Test]
    public async Task ManageDeletions_WhenAllowed_Succeeds()
    {
        // Arrange

        // The value for `authenticationType` parameter causes `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] { new(ClaimTypes.Role, RoleName.Admin) }, "Basic"));
        var context = new AuthorizationHandlerContext(_requirements, user, EmptyCustomerView);
        var handler = new CustomerViewPermissionsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAuthenticated_DoesNotSucceed()
    {
        // Arrange

        // This `ClaimsPrincipal` is not authenticated.
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] { new(ClaimTypes.Role, RoleName.Admin) }));
        var context = new AuthorizationHandlerContext(_requirements, user, EmptyCustomerView);
        var handler = new CustomerViewPermissionsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }

    [Test]
    public async Task ManageDeletions_WhenNotAllowed_DoesNotSucceed()
    {
        // Arrange

        // This `ClaimsPrincipal` is authenticated but does not have the Admin role.
        var user = new ClaimsPrincipal(new ClaimsIdentity("Basic"));
        var context = new AuthorizationHandlerContext(_requirements, user, EmptyCustomerView);
        var handler = new CustomerViewPermissionsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }
}
