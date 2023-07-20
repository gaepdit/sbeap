using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Identity;
using Sbeap.TestData.Constants;

namespace AppServicesTests.ActionItemTypes;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new ActionItemType(Guid.NewGuid(), TextData.ValidName);
        var repoMock = new Mock<IActionItemTypeRepository>();
        var managerMock = new Mock<IActionItemTypeManager>();
        managerMock.Setup(l =>
                l.CreateAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new ActionItemTypeService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);
        var resource = new ActionItemTypeCreateDto(TextData.ValidName);

        var result = await appService.CreateAsync(resource);

        result.Should().Be(item.Id);
    }
}
