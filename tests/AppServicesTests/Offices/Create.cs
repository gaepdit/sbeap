﻿using Sbeap.AppServices.Offices;
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
        var repoMock = Substitute.For<IOfficeRepository>();
        var managerMock = Substitute.For<IOfficeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Is((string?)null), Arg.Any<CancellationToken>()).Returns(item);
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns((ApplicationUser?)null);
        var appService = new OfficeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);
        var resource = new OfficeCreateDto(TextData.ValidName);

        var result = await appService.CreateAsync(resource);

        result.Should().Be(item.Id);
    }
}
