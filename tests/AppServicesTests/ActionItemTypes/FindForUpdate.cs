using AutoMapper;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData.Constants;

namespace AppServicesTests.ActionItemTypes;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var ActionItemType = new ActionItemType(Guid.Empty, TextData.ValidName);
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.FindAsync(ActionItemType.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ActionItemType);
        var managerMock = new Mock<IActionItemTypeManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new ActionItemTypeService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);

        var result = await appService.FindActionItemTypeForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(ActionItemType);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionItemType?)null);
        var managerMock = new Mock<IActionItemTypeManager>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new ActionItemTypeService(repoMock.Object, managerMock.Object,
            mapperMock.Object, userServiceMock.Object);

        var result = await appService.FindActionItemTypeForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
