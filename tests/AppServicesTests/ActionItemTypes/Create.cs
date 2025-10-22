using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.AuthenticationServices;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Identity;
using Sbeap.TestData.Constants;

namespace AppServicesTests.ActionItemTypes;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        // Arrange
        var item = new ActionItemType(Guid.NewGuid(), TextData.ValidName);
        var resource = new ActionItemTypeCreateDto(TextData.ValidName);

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        var managerMock = Substitute.For<IActionItemTypeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Is((string?)null), Arg.Any<CancellationToken>())
            .Returns(item);
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var appService = new ActionItemTypeService(repoMock, managerMock, AppServicesTestsSetup.Mapper!,
            userServiceMock, Substitute.For<IMemoryCache>());

        // Act
        var result = await appService.CreateAsync(resource);

        // Assert
        result.Should().Be(item.Id);
    }
}
