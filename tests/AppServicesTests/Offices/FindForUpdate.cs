using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.AuthenticationServices;
using Sbeap.AppServices.Offices;
using Sbeap.Domain.Entities.Offices;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Offices;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var office = new Office(Guid.Empty, TextData.ValidName);
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindAsync(office.Id, Arg.Any<CancellationToken>()).Returns(office);
        var appService = new OfficeService(repoMock, Substitute.For<IOfficeManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), new MemoryCache(new MemoryCacheOptions()));

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(office);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>()).Returns((Office?)null);
        var appService = new OfficeService(repoMock, Substitute.For<IOfficeManager>(), Substitute.For<IMapper>(),
            Substitute.For<IUserService>(), new MemoryCache(new MemoryCacheOptions()));

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
