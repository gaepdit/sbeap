using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Identity;
using System.Security.Claims;

namespace AppServicesTests.Cases;

public class CaseworkViewPermissions
{
    private readonly CaseworkOperation[] _requirements = { CaseworkOperation.ManageDeletions };

    private static CustomerSearchResultDto EmptyCustomer => new(Guid.Empty, string.Empty, string.Empty, null, false);
    private static CaseworkViewDto EmptyCaseworkView => new() { Customer = EmptyCustomer };

    [Test]
    public async Task ManageDeletions_WhenAllowed_Succeeds()
    {
        // Arrange

        // The value for `authenticationType` parameter causes `ClaimsIdentity.IsAuthenticated` to be set to `true`.
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] { new(ClaimTypes.Role, RoleName.Admin) }, "Basic"));
        var context = new AuthorizationHandlerContext(_requirements, user, EmptyCaseworkView);
        var handler = new CaseworkViewPermissionsHandler();

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
        var context = new AuthorizationHandlerContext(_requirements, user, EmptyCaseworkView);
        var handler = new CaseworkViewPermissionsHandler();

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
        var context = new AuthorizationHandlerContext(_requirements, user, EmptyCaseworkView);
        var handler = new CaseworkViewPermissionsHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }
}
