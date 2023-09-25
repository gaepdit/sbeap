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
public class EditActionModel : PageModel
{
    // Constructor
    private readonly IActionItemService _service;
    private readonly ICaseworkService _cases;
    private readonly IActionItemTypeService _actionItemTypes;
    private readonly IAuthorizationService _authorization;

    public EditActionModel(IActionItemService service, ICaseworkService cases, IActionItemTypeService actionItemTypes,
        IAuthorizationService authorization)
    {
        _service = service;
        _cases = cases;
        _authorization = authorization;
        _actionItemTypes = actionItemTypes;
    }

    // Properties
    [BindProperty]
    public ActionItemUpdateDto Item { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CaseworkSearchResultDto CaseView { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public SelectList ActionItemTypeSelectList { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? caseId, Guid? actionId)
    {
        if (caseId is null || actionId is null) return RedirectToPage("../Index");

        var caseView = await _cases.FindBasicInfoAsync(caseId.Value);
        if (caseView is null) return NotFound();
        CaseView = caseView;

        var actionItem = await _service.FindForUpdateAsync(actionId.Value);
        if (actionItem is null || actionItem.CaseWorkId != caseId) return NotFound();
        Item = actionItem;

        await SetPermissionAsync(actionItem);

        if (UserCan[CaseworkOperation.EditActionItems])
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        if (!UserCan[CaseworkOperation.ManageDeletions] || Item.IsDeleted)
            return NotFound();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted case.");
        return RedirectToPage("Details", new { id = caseId });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var originalActionItem = await _service.FindForUpdateAsync(Item.Id);
        if (originalActionItem is null) return BadRequest();

        await SetPermissionAsync(originalActionItem);
        if (!UserCan[CaseworkOperation.EditActionItems]) return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await _service.UpdateAsync(Item);

        HighlightId = Item.Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Action Item successfully updated.");
        return RedirectToPage("Details", new { id = originalActionItem.CaseWorkId });
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await _actionItemTypes.GetListItemsAsync()).ToSelectList();

    private async Task SetPermissionAsync(ActionItemUpdateDto item)
    {
        foreach (var operation in CaseworkOperation.AllOperations)
            UserCan[operation] = (await _authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
