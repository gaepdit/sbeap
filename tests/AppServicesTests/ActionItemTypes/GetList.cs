﻿using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData.Constants;

namespace AppServicesTests.ActionItemTypes;

public class GetList
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        var actionItemType = new ActionItemType(Guid.Empty, TextData.ValidName);
        var itemList = new List<ActionItemType> { actionItemType };

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.GetListAsync(Arg.Any<CancellationToken>()).Returns(itemList);
        var managerMock = Substitute.For<IActionItemTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ActionItemTypeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.GetListAsync(Arg.Any<CancellationToken>()).Returns(new List<ActionItemType>());
        var managerMock = Substitute.For<IActionItemTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new ActionItemTypeService(repoMock, managerMock,
            AppServicesTestsSetup.Mapper!, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEmpty();
    }
}
