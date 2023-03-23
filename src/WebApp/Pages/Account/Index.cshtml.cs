using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Staff;
using Sbeap.Domain.Identity;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.RazorHelpers;

namespace Sbeap.WebApp.Pages.Account;

[Authorize]
public class IndexModel : PageModel
{
    public StaffViewDto DisplayStaff { get; private set; } = default!;
    public string? OfficeName => DisplayStaff.Office?.Name;
    public IList<AppRole> Roles { get; private set; } = default!;
    public DisplayMessage? Message { get; private set; }

    public async Task<IActionResult> OnGetAsync([FromServices] IStaffAppService staffService)
    {
        var staff = await staffService.GetCurrentUserAsync();
        if (staff is null) return Forbid();

        DisplayStaff = staff;
        Roles = await staffService.GetAppRolesAsync(DisplayStaff.Id);
        Message = TempData.GetDisplayMessage();

        return Page();
    }
}
