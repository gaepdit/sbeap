using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using MyAppRoot.WebApp.Api;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.TestData.Constants;

namespace WebAppTests.Api;

[TestFixture]
public class ActionItemTypeApiTests
{
    private static ActionItemTypeViewDto ValidActionItemTypeView => new(Guid.Empty, TextData.ValidName, true);

    [Test]
    public async Task ListActionItemTypes_ReturnsListOfActionItemTypes()
    {
        List<ActionItemTypeViewDto> actionItemTypeList = new() { ValidActionItemTypeView };
        var service = new Mock<IActionItemTypeService>();
        service.Setup(l => l.GetListAsync(CancellationToken.None))
            .ReturnsAsync(actionItemTypeList);
        var apiController = new ActionItemTypeApiController(service.Object);

        var result = await apiController.ListActionItemTypesServiceAsync();

        result.Should().BeEquivalentTo(actionItemTypeList);
    }

    // Test Not correct
    //[Test]
    //public async Task GetActionItemType_ReturnsActionItemTypeView()
    //{
    //    var service = new Mock<IActionItemTypeService>();
    //    service.Setup(l => l.FindAsync(Guid.Empty, CancellationToken.None))
    //        .ReturnsAsync(ValidActionItemTypeView);
    //    var apiController = new ActionItemTypeApiController(service.Object);

    //    var response = await apiController.GetActionItemTypeAsync(Guid.Empty);

    //    using (new AssertionScope())
    //    {
    //        response.Result.Should().BeOfType<OkObjectResult>();
    //        var result = response.Result as OkObjectResult;
    //        result.Should().NotBeNull();
    //        result?.Value.Should().Be(ValidActionItemTypeView);
    //    }
    //}

    [Test]
    public async Task GetActionItemType_UnknownIdReturnsNotFound()
    {
        var service = new Mock<IActionItemTypeService>();
        service.Setup(l => l.FindAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(null as ActionItemTypeViewDto);
        var apiController = new ActionItemTypeApiController(service.Object);

        var response = await apiController.GetActionItemTypeAsync(Guid.Empty);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<ObjectResult>();
            var result = response.Result as ObjectResult;
            result?.StatusCode.Should().Be(404);
        }
    }
}
