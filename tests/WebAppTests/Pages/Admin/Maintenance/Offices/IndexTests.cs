using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using Sbeap.AppServices.Offices;
using Sbeap.TestData.Constants;
using Sbeap.WebApp.Pages.Admin.Maintenance.Offices;
using Sbeap.WebApp.Platform.PageModelHelpers;
using System.Security.Claims;

namespace WebAppTests.Pages.Admin.Maintenance.Offices;

public class IndexTests
{
    private static readonly List<OfficeViewDto> ListTest = new()
        { new OfficeViewDto { Id = Guid.Empty, Name = TextData.ValidName } };

    [Test]
    public async Task OnGet_ReturnsWithList()
    {
        var serviceMock = new Mock<IOfficeService>();
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
