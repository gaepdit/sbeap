using Sbeap.AppServices.Offices;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Offices;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new Office(Guid.NewGuid(), TextData.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        var managerMock = new Mock<IOfficeManager>();
        managerMock.Setup(l =>
                l.CreateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(l => l.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);
        var appService = new OfficeAppService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);
        var resource = new OfficeCreateDto { Name = TextData.ValidName };

        var result = await appService.CreateAsync(resource);

        result.Should().Be(item.Id);
    }
}
