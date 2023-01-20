using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.TestData.Constants;
using MyAppRoot.WebApp.Api;

namespace WebAppTests.Api;

[TestFixture]
public class OfficeApiTests
{
    [Test]
    public async Task ListOffices_ReturnsListOfOffices()
    {
        List<OfficeViewDto> officeList = new()
            { new OfficeViewDto { Id = Guid.Empty, Name = TestConstants.ValidName } };
        var service = new Mock<IOfficeAppService>();
        service.Setup(l => l.GetListAsync(CancellationToken.None))
            .ReturnsAsync(officeList);
        var apiController = new OfficeApiController(service.Object);

        var result = await apiController.ListOfficesAsync();

        result.Should().BeEquivalentTo(officeList);
    }

    [Test]
    public async Task GetOffice_ReturnsOfficeView()
    {
        var item = Mock.Of<OfficeViewDto>();
        var service = new Mock<IOfficeAppService>();
        service.Setup(l => l.FindAsync(Guid.Empty, CancellationToken.None))
            .ReturnsAsync(item);
        var apiController = new OfficeApiController(service.Object);

        var response = await apiController.GetOfficeAsync(Guid.Empty);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result.Should().NotBeNull();
            result?.Value.Should().Be(item);
        }
    }

    [Test]
    public async Task GetOffice_UnknownIdReturnsNotFound()
    {
        var service = new Mock<IOfficeAppService>();
        service.Setup(l => l.FindAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(null as OfficeViewDto);
        var apiController = new OfficeApiController(service.Object);

        var response = await apiController.GetOfficeAsync(Guid.Empty);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<ObjectResult>();
            var result = response.Result as ObjectResult;
            result?.StatusCode.Should().Be(404);
        }
    }
}
