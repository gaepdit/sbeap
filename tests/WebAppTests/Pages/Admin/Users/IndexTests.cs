using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.WebApp.Pages.Admin.Users;

namespace WebAppTests.Pages.Admin.Users;

public class IndexTests
{
    private static StaffSearchDto DefaultStaffSearch => new(SortBy.NameAsc, null, null, null, null, null);

    [Test]
    public async Task OnSearch_IfValidModel_ReturnsPage()
    {
        // Arrange
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetActiveListItemsAsync(Arg.Any<CancellationToken>()).Returns(new List<ListItem>());

        var paging = new PaginatedRequest(1, 1, "Id");
        var output = new PaginatedResult<StaffSearchResultDto>(new List<StaffSearchResultDto>(), 1, paging);
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.SearchAsync(Arg.Any<StaffSearchDto>(), Arg.Any<PaginatedRequest>()).Returns(output);

        var page = new IndexModel(officeServiceMock, staffServiceMock)
            { TempData = WebAppTestsSetup.PageTempData() };

        // Act
        var result = await page.OnGetSearchAsync(DefaultStaffSearch);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeTrue();
            page.SearchResults.Should().Be(output);
            page.SearchResults.Items.Should().BeEmpty();
            page.ShowResults.Should().BeTrue();
        }
    }

    [Test]
    public async Task OnSearch_IfInvalidModel_ReturnPageWithInvalidModelState()
    {
        var officeServiceMock = Substitute.For<IOfficeService>();
        officeServiceMock.GetActiveListItemsAsync(Arg.Any<CancellationToken>()).Returns(new List<ListItem>());
        var staffServiceMock = Substitute.For<IStaffService>();
        var page = new IndexModel(officeServiceMock, staffServiceMock)
            { TempData = WebAppTestsSetup.PageTempData() };
        page.ModelState.AddModelError("Error", "Sample error description");

        var result = await page.OnGetSearchAsync(DefaultStaffSearch);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
        }
    }
}
