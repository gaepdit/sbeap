using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Agencies;
using Sbeap.TestData.Constants;
using Sbeap.WebApp.Pages.Admin.Maintenance.Agency;
using Sbeap.WebApp.Platform.PageModelHelpers;
using System.Security.Claims;

namespace WebAppTests.Pages.Admin.Maintenance.Agencies;

public class IndexTests
{
    private static readonly List<AgencyViewDto> ListTest = new()
        { new AgencyViewDto(Guid.Empty, TextData.ValidName, true) };

    [Test]
    public async Task OnGet_ReturnsWithList()
    {
        var serviceMock = new Mock<IAgencyService>();
        serviceMock.Setup(l => l.GetListAsync(CancellationToken.None)).ReturnsAsync(ListTest);
        var authorizationMock = new Mock<IAuthorizationService>();
        authorizationMock.Setup(l => l.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, It.IsAny<string>()))
            .ReturnsAsync(AuthorizationResult.Success);
        var page = new IndexModel { TempData = WebAppTestsSetup.PageTempData() };

        await page.OnGetAsync(serviceMock.Object, authorizationMock.Object);

        using (new AssertionScope())
        {
            page.Items.Should().BeEquivalentTo(ListTest);
            page.TempData.GetDisplayMessage().Should().BeNull();
            page.HighlightId.Should().BeNull();
        }
    }
}
