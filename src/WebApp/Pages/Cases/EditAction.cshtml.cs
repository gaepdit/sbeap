using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Cases;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class EditActionModel(
    IActionItemService service,
    ICaseworkService cases,
    IActionItemTypeService actionItemTypes,
    IAuthorizationService authorization)
    : PageModel
{
    // Properties

    [FromRoute]
    public Guid ActionId { get; set; }

    [BindProperty]
    public ActionItemUpdateDto ActionItemUpdate { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CaseworkSearchResultDto CaseView { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public SelectList ActionItemTypeSelectList { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? actionId)
    {
        if (actionId is null) return RedirectToPage("../Index");

        var actionItem = await service.FindForUpdateAsync(actionId.Value);
        if (actionItem is null) return NotFound();

        var caseView = await cases.FindBasicInfoAsync(actionItem.CaseWorkId);
        if (caseView is null) return NotFound();

        await SetPermissionsAsync(actionItem);

        if (UserCan[CaseworkOperation.EditActionItems])
        {
            CaseView = caseView;
            ActionId = actionId.Value;
            ActionItemUpdate = actionItem;
            await PopulateSelectListsAsync();
            return Page();
        }

        if (!UserCan[CaseworkOperation.ManageDeletions] || ActionItemUpdate.IsDeleted)
            return NotFound();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted case.");
        return RedirectToPage("Details", new { id = actionItem.CaseWorkId });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var originalActionItem = await service.FindForUpdateAsync(ActionId);
        if (originalActionItem is null) return BadRequest();

        await SetPermissionsAsync(originalActionItem);
        if (!UserCan[CaseworkOperation.EditActionItems]) return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await service.UpdateAsync(ActionId, ActionItemUpdate);

        HighlightId = ActionId;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Action Item successfully updated.");
        return RedirectToPage("Details", new { id = originalActionItem.CaseWorkId });
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await actionItemTypes.GetActiveListItemsAsync()).ToSelectList();

    private async Task SetPermissionsAsync(ActionItemUpdateDto item)
    {
        foreach (var operation in CaseworkOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
