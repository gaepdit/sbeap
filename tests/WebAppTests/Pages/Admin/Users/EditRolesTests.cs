using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.Domain.Identity;
using Sbeap.WebApp.Pages.Admin.Users;

namespace WebAppTests.Pages.Admin.Users;

public class EditRolesTests
{
    private static readonly OfficeViewDto OfficeViewTest = new(Guid.Empty, TextData.ValidName, true);

    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.Empty.ToString(),
        FamilyName = TextData.ValidName,
        GivenName = TextData.ValidName,
        Email = TextData.ValidEmail,
        Office = OfficeViewTest,
        Active = true,
    };

    private static readonly List<EditRolesModel.RoleSetting> RoleSettingsTest = new()
    {
        new EditRolesModel.RoleSetting
        {
            Name = TextData.ValidName,
            DisplayName = TextData.ValidName,
            Description = TextData.ValidName,
            IsSelected = true,
        },
    };

    [Test]
    public async Task OnGet_PopulatesThePageModel()
    {
        var expectedRoleSettings = AppRole.AllRoles
            .Select(r => new EditRolesModel.RoleSetting
            {
                Name = r.Key,
                DisplayName = r.Value.DisplayName,
                Description = r.Value.Description,
                IsSelected = r.Key == RoleName.SiteMaintenance,
            }).ToList();

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns(StaffViewTest);
        staffServiceMock.GetRolesAsync(Arg.Any<string>()).Returns(new List<string> { RoleName.SiteMaintenance });
        var pageModel = new EditRolesModel(staffServiceMock) { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(StaffViewTest.Id);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(StaffViewTest);
            pageModel.OfficeName.Should().Be(TextData.ValidName);
            pageModel.UserId.Should().Be(Guid.Empty.ToString());
            pageModel.RoleSettings.Should().BeEquivalentTo(expectedRoleSettings);
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        var pageModel = new EditRolesModel(staffServiceMock) { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(null);

        using (new AssertionScope())
        {
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Index");
        }
    }

    [Test]
    public async Task OnGet_NonexistentIdReturnsNotFound()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns((StaffViewDto?)null);
        var pageModel = new EditRolesModel(staffServiceMock) { TempData = WebAppTestsSetup.PageTempData() };

        var result = await pageModel.OnGetAsync(Guid.Empty.ToString());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.");

        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateRolesAsync(Arg.Any<string>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Success);
        staffServiceMock.GetRolesAsync(Arg.Any<string>()).Returns(new List<string> { RoleName.SiteMaintenance });
        var page = new EditRolesModel(staffServiceMock)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsSetup.PageTempData(),
        };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            page.ModelState.IsValid.Should().BeTrue();
            result.Should().BeOfType<RedirectToPageResult>();
            ((RedirectToPageResult)result).PageName.Should().Be("Details");
            ((RedirectToPageResult)result).RouteValues!["id"].Should().Be(Guid.Empty.ToString());
            page.TempData.GetDisplayMessage().Should().BeEquivalentTo(expectedMessage);
        }
    }

    [Test]
    public async Task OnPost_GivenMissingUser_ReturnsBadRequest()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateRolesAsync(Arg.Any<string>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Failed());
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns((StaffViewDto?)null);
        var page = new EditRolesModel(staffServiceMock)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsSetup.PageTempData(),
        };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsPageWithInvalidModelState()
    {
        var staffServiceMock = Substitute.For<IStaffService>();
        staffServiceMock.UpdateRolesAsync(Arg.Any<string>(), Arg.Any<Dictionary<string, bool>>())
            .Returns(IdentityResult.Failed(new IdentityError { Code = "CODE", Description = "DESCRIPTION" }));
        staffServiceMock.FindAsync(Arg.Any<string>()).Returns(StaffViewTest);
        staffServiceMock.GetRolesAsync(Arg.Any<string>()).Returns(new List<string> { RoleName.SiteMaintenance });
        var page = new EditRolesModel(staffServiceMock)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsSetup.PageTempData(),
        };

        var result = await page.OnPostAsync();

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            page.ModelState.IsValid.Should().BeFalse();
            page.ModelState[string.Empty]!.Errors[0].ErrorMessage.Should().Be("CODE: DESCRIPTION");
            page.DisplayStaff.Should().Be(StaffViewTest);
            page.UserId.Should().Be(Guid.Empty.ToString());
            page.RoleSettings.Should().BeEquivalentTo(RoleSettingsTest);
        }
    }
}
