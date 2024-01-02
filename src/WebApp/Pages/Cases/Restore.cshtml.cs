using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Cases;

[Authorize(Policy = nameof(Policies.AdminUser))]
public class RestoreModel(ICaseworkService service, IAuthorizationService authorization)
    : PageModel
{
    // Properties
    [BindProperty]
    public Guid Id { get; set; }

    public CaseworkViewDto Item { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await service.FindAsync(id.Value);
        if (item is null) return NotFound();
        if (!await UserCanManageDeletionsAsync(item)) return Forbid();

        if (!item.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Case is not deleted.");
            return RedirectToPage("Details", new { id });
        }

        Id = id.Value;
        Item = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var item = await service.FindAsync(Id);
        if (item is null) return BadRequest();
        if (!await UserCanManageDeletionsAsync(item)) return BadRequest();

        if (!item.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Case is not deleted.");
            return RedirectToPage("Details", new { Id });
        }

        if (!ModelState.IsValid)
        {
            Item = item;
            return Page();
        }

        await service.RestoreAsync(Id);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Case successfully restored.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task<bool> UserCanManageDeletionsAsync(CaseworkViewDto item) =>
        (await authorization.AuthorizeAsync(User, item, CaseworkOperation.ManageDeletions)).Succeeded;
}
