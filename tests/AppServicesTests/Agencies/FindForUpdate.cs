using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
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
        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), new MemoryCache(new MemoryCacheOptions()));

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(agency);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>()).Returns((Agency?)null);
        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), Substitute.For<IMapper>(),
            Substitute.For<IUserService>(), new MemoryCache(new MemoryCacheOptions()));

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
