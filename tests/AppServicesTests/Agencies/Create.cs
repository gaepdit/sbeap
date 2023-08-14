﻿using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Identity;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Agencies;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new Agency(Guid.NewGuid(), TextData.ValidName);
        var repoMock = Substitute.For<IAgencyRepository>();
        var managerMock = Substitute.For<IAgencyManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Is((string?)null), Arg.Any<CancellationToken>()).Returns(item);
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns((ApplicationUser?)null);
        var appService = new AgencyService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);
        var resource = new AgencyCreateDto(TextData.ValidName);

        var result = await appService.CreateAsync(resource);

        result.Should().Be(item.Id);
    }
}
