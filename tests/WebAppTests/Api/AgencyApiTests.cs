using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Sbeap.AppServices.Agencies;
using Sbeap.TestData.Constants;
using Sbeap.WebApp.Api;

namespace WebAppTests.Api;

[TestFixture]
public class AgencyApiTests
{
    private static AgencyViewDto ValidAgencyView => new(Guid.Empty, TextData.ValidName, true);

    [Test]
    public async Task ListAgencies_ReturnsListOfAgencies()
    {
        List<AgencyViewDto> agencyList = new() { ValidAgencyView };
        var service = new Mock<IAgencyService>();
        service.Setup(l => l.GetListAsync(CancellationToken.None))
            .ReturnsAsync(agencyList);
        var apiController = new AgencyApiController(service.Object);

        var result = await apiController.ListAgencyServiceAsync();

        result.Should().BeEquivalentTo(agencyList);
    }

    [Test]
    public async Task GetAgency_ReturnsAgencyView()
    {
        var service = new Mock<IAgencyService>();
        service.Setup(l => l.FindAsync(Guid.Empty, CancellationToken.None))
            .ReturnsAsync(ValidAgencyView);
        var apiController = new AgencyApiController(service.Object);

        var response = await apiController.GetAsync(Guid.Empty);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = response.Result as OkObjectResult;
            result.Should().NotBeNull();
            result?.Value.Should().Be(ValidAgencyView);
        }
    }

    [Test]
    public async Task GetAgency_UnknownIdReturnsNotFound()
    {
        var service = new Mock<IAgencyService>();
        service.Setup(l => l.FindAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(null as AgencyViewDto);
        var apiController = new AgencyApiController(service.Object);

        var response = await apiController.GetAsync(Guid.Empty);

        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<ObjectResult>();
            var result = response.Result as ObjectResult;
            result?.StatusCode.Should().Be(404);
        }
    }
}
