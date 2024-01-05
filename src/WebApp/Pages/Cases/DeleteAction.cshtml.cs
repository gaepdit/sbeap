using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Cases;

public class DeleteActionModel(IActionItemService service, ICaseworkService cases, IAuthorizationService authorization)
    : PageModel
{
    // Properties

    [BindProperty]
    public Guid ActionItemId { get; set; }

    public ActionItemViewDto ActionItemView { get; private set; } = default!;
    public CaseworkViewDto CaseView { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("Index");

        var actionItem = await service.FindAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var caseView = await cases.FindAsync(actionItem.CaseWorkId);
        if (caseView is null) return NotFound();

        if (!await UserCanDeleteActionItemsAsync(caseView)) return Forbid();

        CaseView = caseView;
        ActionItemView = actionItem;
        ActionItemId = actionId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalActionItem = await service.FindAsync(ActionItemId);
        if (originalActionItem is null) return BadRequest();

        var caseView = await cases.FindAsync(originalActionItem.CaseWorkId);
        if (caseView is null || caseView.IsDeleted || !await UserCanDeleteActionItemsAsync(caseView))
            return BadRequest();

        await service.DeleteAsync(ActionItemId);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Action Item successfully deleted.");
        return RedirectToPage("Details", new { caseView.Id });
    }

    private async Task<bool> UserCanDeleteActionItemsAsync(CaseworkViewDto item) =>
        (await authorization.AuthorizeAsync(User, item, CaseworkOperation.EditActionItems)).Succeeded;
}
