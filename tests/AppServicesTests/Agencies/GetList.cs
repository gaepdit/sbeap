using Microsoft.Extensions.Caching.Memory;
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
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.GetListAsync(Arg.Any<string?>(), Arg.Any<string[]?>(), Arg.Any<CancellationToken>())
            .Returns(itemList);
        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), new MemoryCache(new MemoryCacheOptions()));

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.GetListAsync(Arg.Any<string?>(), Arg.Any<string[]?>(), Arg.Any<CancellationToken>())
            .Returns(new List<Agency>());
        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), new MemoryCache(new MemoryCacheOptions()));

        var result = await appService.GetListAsync();

        result.Should().BeEmpty();
    }
}
