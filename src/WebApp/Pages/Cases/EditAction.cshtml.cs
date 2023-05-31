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

[Authorize(Policy = PolicyName.StaffUser)]
public class EditActionModel : PageModel
{
    // Constructor
    private readonly IActionItemService _service;
    private readonly ICaseworkService _cases;
    private readonly IActionItemTypeService _actionItemTypes;
    private readonly IAuthorizationService _authorization;

    public EditActionModel(
        IActionItemService service,
        ICaseworkService cases,
        IActionItemTypeService actionItemTypes,
        IAuthorizationService authorization)
    {
        _service = service;
        _cases = cases;
        _authorization = authorization;
        _actionItemTypes = actionItemTypes;
    }

    // Properties
    [BindProperty]
    public ActionItemUpdateDto ActionItemUpdate { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CaseworkSearchResultDto CaseView { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public SelectList ActionItemTypeSelectList { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id, Guid? actionId)
    {
        if (id is null || actionId is null) return RedirectToPage("../Index");

        var casework = await _cases.FindBasicInfoAsync(id.Value);
        if (casework is null) return NotFound();
        CaseView = casework;

        var action = await _service.FindActionItemForUpdateAsync(actionId.Value);
        if (action is null) return NotFound();
        ActionItemUpdate = action;

        foreach (var operation in CaseworkOperation.AllOperations) await SetPermissionAsync(operation);

        if (UserCan[CaseworkOperation.EditActionItems])
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        if (!UserCan[CaseworkOperation.ManageDeletions]) return NotFound();
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted case or action item.");
        return RedirectToPage("Details", new { id });
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");

        foreach (var operation in CaseworkOperation.AllOperations) await SetPermissionAsync(operation);
        if (!UserCan[CaseworkOperation.EditActionItems]) return Forbid();

        var casework = await _cases.FindBasicInfoAsync(id.Value);
        if (casework is null) return BadRequest();

        CaseView = casework;
        if (CaseView.IsDeleted) return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await _service.UpdateActionItemAsync(ActionItemUpdate);

        HighlightId = ActionItemUpdate.Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Action Item successfully updated.");
        return RedirectToPage("Details", new { id });
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await _actionItemTypes.GetListItemsAsync()).ToSelectList();

    private async Task SetPermissionAsync(IAuthorizationRequirement operation) =>
        UserCan[operation] = (await _authorization.AuthorizeAsync(User, ActionItemUpdate, operation)).Succeeded;
}
