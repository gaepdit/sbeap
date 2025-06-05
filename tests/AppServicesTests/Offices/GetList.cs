using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Offices;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Offices;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        var office = new Office(Guid.Empty, TextData.ValidName);
        var itemList = new List<Office> { office };
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetListAsync(Arg.Any<CancellationToken>()).Returns(itemList);
        var appService = new OfficeService(repoMock, Substitute.For<IOfficeManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), new MemoryCache(new MemoryCacheOptions()));

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetListAsync(Arg.Any<CancellationToken>()).Returns(new List<Office>());
        var appService = new OfficeService(repoMock, Substitute.For<IOfficeManager>(), AppServicesTestsSetup.Mapper!,
            Substitute.For<IUserService>(), new MemoryCache(new MemoryCacheOptions()));

        var result = await appService.GetListAsync();

        result.Should().BeEmpty();
    }
}
