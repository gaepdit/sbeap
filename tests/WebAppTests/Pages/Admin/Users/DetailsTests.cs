using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.Domain.Identity;
using Sbeap.TestData.Constants;
using Sbeap.WebApp.Pages.Admin.Users;
using System.Security.Claims;

namespace WebAppTests.Pages.Admin.Users;

public class DetailsTests
{
    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var staffView = new StaffViewDto
        {
            Id = Guid.Empty.ToString(),
            FamilyName = TextData.ValidName,
            GivenName = TextData.ValidName,
            Email = TextData.ValidEmail,
            Active = true,
        };

        var serviceMock = Substitute.For<IStaffService>();
        serviceMock.FindAsync(Arg.Any<string>()).Returns(staffView);
        serviceMock.GetAppRolesAsync(Arg.Any<string>()).Returns(new List<AppRole>());
        var authorizationMock = Substitute.For<IAuthorizationService>();
        authorizationMock.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Is((string?)null), Arg.Any<string>())
            .Returns(AuthorizationResult.Success());
        var pageModel = new DetailsModel { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(serviceMock, authorizationMock, staffView.Id);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(staffView);
            pageModel.Roles.Should().BeEmpty();
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var serviceMock = Substitute.For<IStaffService>();
        var pageModel = new DetailsModel { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(serviceMock, Substitute.For<IAuthorizationService>(), null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var serviceMock = Substitute.For<IStaffService>();
        serviceMock.FindAsync(Arg.Any<string>()).Returns((StaffViewDto?)null);
        var pageModel = new DetailsModel { TempData = WebAppTestsSetup.PageTempData() };

        var result =
            await pageModel.OnGetAsync(serviceMock, Substitute.For<IAuthorizationService>(), Guid.Empty.ToString());

        result.Should().BeOfType<NotFoundResult>();
    }
}
