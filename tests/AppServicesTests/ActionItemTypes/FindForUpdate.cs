using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.AuthenticationServices;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData.Constants;

namespace AppServicesTests.ActionItemTypes;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var actionItemType = new ActionItemType(Guid.Empty, TextData.ValidName);

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindAsync(actionItemType.Id, Arg.Any<CancellationToken>()).Returns(actionItemType);
        var managerMock = Substitute.For<IActionItemTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();

        var appService = new ActionItemTypeService(repoMock, managerMock, AppServicesTestsSetup.Mapper!,
            userServiceMock, Substitute.For<IMemoryCache>());

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        // Assert
        result.Should().BeEquivalentTo(actionItemType);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        // Arrange
        var id = Guid.Empty;

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var managerMock = Substitute.For<IActionItemTypeManager>();
        var mapperMock = Substitute.For<IMapper>();
        var userServiceMock = Substitute.For<IUserService>();

        var appService = new ActionItemTypeService(repoMock, managerMock, mapperMock, userServiceMock,
            Substitute.For<IMemoryCache>());

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }
}
