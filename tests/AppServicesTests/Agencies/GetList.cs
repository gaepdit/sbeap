using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Agencies;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        var agency = new Agency(Guid.Empty, TextData.ValidName);
        var itemList = new List<Agency> { agency };

        var repoMock = new Mock<IAgencyRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(itemList);
        var managerMock = new Mock<IAgencyManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new AgencyService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);

        var result = await appService.GetListItemsAsync();

        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        var repoMock = new Mock<IAgencyRepository>();
        repoMock.Setup(l => l.GetListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Agency>());
        var managerMock = new Mock<IAgencyManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new AgencyService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);

        var result = await appService.GetListItemsAsync();

        result.Should().BeEmpty();
    }
}
