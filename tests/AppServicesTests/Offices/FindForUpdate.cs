using AutoMapper;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Identity;
using Sbeap.Domain.Offices;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Offices;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var office = new Office(Guid.Empty, TestConstants.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindAsync(office.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(office);
        var managerMock = new Mock<IOfficeManager>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsGlobal.Mapper!, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(office);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var managerMock = new Mock<IOfficeManager>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            mapperMock.Object, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
