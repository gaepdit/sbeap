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
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindAsync(agency.Id, Arg.Any<CancellationToken>()).Returns(agency);
        var managerMock = Substitute.For<IAgencyManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new AgencyService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(agency);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>()).Returns((Agency?)null);
        var managerMock = Substitute.For<IAgencyManager>();
        var mapperMock = Substitute.For<IMapper>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new AgencyService(repoMock, managerMock,
            mapperMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
