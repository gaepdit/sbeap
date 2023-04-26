﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Permissions;
using Sbeap.AppServices.Staff;
using Sbeap.Domain.Identity;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Admin.Users;

[Authorize]
public class DetailsModel : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = default!;
    public string? OfficeName => DisplayStaff.Office?.Name;
    public IList<AppRole> Roles { get; private set; } = default!;
    public DisplayMessage? Message { get; private set; }
    public bool IsUserAdministrator { get; private set; }

    public async Task<IActionResult> OnGetAsync(
        [FromServices] IStaffAppService staffService,
        [FromServices] IAuthorizationService authorization,
        string? id)
    {
        if (id is null) return RedirectToPage("Index");
        var staff = await staffService.FindAsync(id);
        if (staff is null) return NotFound();

        DisplayStaff = staff;
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);
        Message = TempData.GetDisplayMessage();
        IsUserAdministrator = (await authorization.AuthorizeAsync(User, PolicyName.UserAdministrator)).Succeeded;

        return Page();
    }
}
