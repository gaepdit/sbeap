﻿using Sbeap.AppServices.Agencies;
using Sbeap.WebApp.Pages.Admin.Maintenance.Agency;

namespace WebAppTests.Pages.Admin.Maintenance.Agencies;

public class IndexTests
{
    private static readonly List<AgencyViewDto> ListTest = new()
        { new AgencyViewDto(Guid.Empty, TextData.ValidName, true) };

    [Test]
    public async Task OnGet_ReturnsWithList()
    {
        var serviceMock = Substitute.For<IAgencyService>();
        serviceMock.GetListAsync(Arg.Any<CancellationToken>()).Returns(ListTest);
        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Is((string?)null), Arg.Any<string>())
            .Returns(AuthorizationResult.Success());
        var page = new IndexModel { TempData = WebAppTestsSetup.PageTempData() };

        await page.OnGetAsync(serviceMock, authorizationMock);

        using (new AssertionScope())
        {
            page.Items.Should().BeEquivalentTo(ListTest);
            page.TempData.GetDisplayMessage().Should().BeNull();
            page.HighlightId.Should().BeNull();
        }
    }
}
