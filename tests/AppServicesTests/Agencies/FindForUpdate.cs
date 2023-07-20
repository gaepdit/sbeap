using AutoMapper;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Agencies;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var agency = new Agency(Guid.Empty, TextData.ValidName);
        var repoMock = new Mock<IAgencyRepository>();
        repoMock.Setup(l => l.FindAsync(agency.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(agency);
        var managerMock = new Mock<IAgencyManager>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new AgencyService(repoMock.Object, managerMock.Object,
            AppServicesTestsSetup.Mapper!, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(agency);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = new Mock<IAgencyRepository>();
        repoMock.Setup(l => l.FindAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Agency?)null);
        var managerMock = new Mock<IAgencyManager>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<IUserService>();
        var appService = new AgencyService(repoMock.Object, managerMock.Object,
            mapperMock.Object, userServiceMock.Object);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
