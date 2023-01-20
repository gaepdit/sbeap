using FluentAssertions.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyAppRoot.AppServices.Offices;
using MyAppRoot.AppServices.Staff;
using MyAppRoot.Domain.Identity;
using MyAppRoot.TestData.Constants;
using MyAppRoot.WebApp.Models;
using MyAppRoot.WebApp.Pages.Admin.Users;
using MyAppRoot.WebApp.Platform.RazorHelpers;

namespace WebAppTests.Pages.Admin.Users;

public class EditRolesTests
{
    private static readonly OfficeViewDto OfficeViewTest = new() { Id = Guid.Empty, Name = TestConstants.ValidName };

    private static readonly StaffViewDto StaffViewTest = new()
    {
        Id = Guid.Empty.ToString(),
        Email = TestConstants.ValidEmail,
        FirstName = TestConstants.ValidName,
        LastName = TestConstants.ValidName,
        Office = OfficeViewTest,
    };

    private static readonly List<EditRolesModel.RoleSetting> RoleSettingsTest = new()
    {
        new EditRolesModel.RoleSetting
        {
            Name = TestConstants.ValidName,
            DisplayName = TestConstants.ValidName,
            Description = TestConstants.ValidName,
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
                IsSelected = r.Key == AppRole.SiteMaintenance,
            }).ToList();

        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync(StaffViewTest);
        staffService.Setup(l => l.GetRolesAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<string> { AppRole.SiteMaintenance });
        var pageModel = new EditRolesModel(staffService.Object) { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(StaffViewTest.Id);

        using (new AssertionScope())
        {
            result.Should().BeOfType<PageResult>();
            pageModel.DisplayStaff.Should().Be(StaffViewTest);
            pageModel.OfficeName.Should().Be(TestConstants.ValidName);
            pageModel.UserId.Should().Be(Guid.Empty.ToString());
            pageModel.RoleSettings.Should().BeEquivalentTo(expectedRoleSettings);
        }
    }

    [Test]
    public async Task OnGet_MissingIdReturnsNotFound()
    {
        var staffService = new Mock<IStaffAppService>();
        var pageModel = new EditRolesModel(staffService.Object) { TempData = WebAppTestsGlobal.GetPageTempData() };

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
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync((StaffViewDto?)null);
        var pageModel = new EditRolesModel(staffService.Object) { TempData = WebAppTestsGlobal.GetPageTempData() };

        var result = await pageModel.OnGetAsync(Guid.Empty.ToString());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task OnPost_GivenSuccess_ReturnsRedirectWithDisplayMessage()
    {
        var expectedMessage =
            new DisplayMessage(DisplayMessage.AlertContext.Success, "User roles successfully updated.");

        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.UpdateRolesAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, bool>>()))
            .ReturnsAsync(IdentityResult.Success);
        staffService.Setup(l => l.GetRolesAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<string> { AppRole.SiteMaintenance });
        var page = new EditRolesModel(staffService.Object)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsGlobal.GetPageTempData(),
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
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.UpdateRolesAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, bool>>()))
            .ReturnsAsync(IdentityResult.Failed());
        staffService.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync((StaffViewDto?)null);
        var page = new EditRolesModel(staffService.Object)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsGlobal.GetPageTempData(),
        };

        var result = await page.OnPostAsync();

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task OnPost_GivenUpdateFailure_ReturnsPageWithInvalidModelState()
    {
        var staffService = new Mock<IStaffAppService>();
        staffService.Setup(l => l.UpdateRolesAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, bool>>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "CODE", Description = "DESCRIPTION" }));
        staffService.Setup(l => l.FindAsync(It.IsAny<string>()))
            .ReturnsAsync(StaffViewTest);
        staffService.Setup(l => l.GetRolesAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<string> { AppRole.SiteMaintenance });
        var page = new EditRolesModel(staffService.Object)
        {
            RoleSettings = RoleSettingsTest,
            UserId = Guid.Empty.ToString(),
            TempData = WebAppTestsGlobal.GetPageTempData(),
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
