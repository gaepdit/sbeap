using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Sbeap.WebApp.Api;
using Sbeap.AppServices.Offices;
using Sbeap.TestData.Constants;

namespace WebAppTests.Api;

[TestFixture]
public class OfficeApiTests
{
    private static OfficeViewDto ValidOfficeView => new(Guid.Empty, TextData.ValidName, true);

    [Test]
    public async Task ListOffices_ReturnsListOfOffices()
    {
        List<OfficeViewDto> officeList = new() { ValidOfficeView };
        var service = new Mock<IOfficeService>();
        service.Setup(l => l.GetListAsync(CancellationToken.None))
            .ReturnsAsync(officeList);
        var apiController = new OfficeApiController(service.Object);

        var result = await apiController.ListOfficesAsync();

        result.Should().BeEquivalentTo(officeList);
    }

    [Test]
    public async Task GetOffice_ReturnsOfficeView()
    {
        var service = new Mock<IOfficeService>();
        service.Setup(l => l.FindAsync(Guid.Empty, CancellationToken.None))
            .ReturnsAsync(ValidOfficeView);
        var apiController = new OfficeApiController(service.Object);

        var response = await apiController.GetOfficeAsync(Guid.Empty);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result.Should().NotBeNull();
            result?.Value.Should().Be(ValidOfficeView);
        }
    }

    [Test]
    public async Task GetOffice_UnknownIdReturnsNotFound()
    {
        var service = new Mock<IOfficeService>();
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
