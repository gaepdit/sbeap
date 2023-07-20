using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData.Constants;

namespace AppServicesTests.ActionItemTypes;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        var actionItemType = new ActionItemType(Guid.Empty, TextData.ValidName);
        var itemList = new List<ActionItemType> { actionItemType };

        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IActionItemTypeManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new ActionItemTypeService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ActionItemType>());
        var managerMock = new Mock<IActionItemTypeManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new ActionItemTypeService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);

        var result = await appService.GetListAsync();

        result.Should().BeEmpty();
    }
}
