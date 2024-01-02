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
public class DetailsModel(
    ICaseworkService cases,
    IActionItemService actionItems,
    IActionItemTypeService actionItemTypes,
    IAuthorizationService authorization)
    : PageModel
{
    // Properties
    public CaseworkViewDto Item { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [BindProperty]
    public ActionItemCreateDto NewActionItem { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    // Select lists
    public SelectList ActionItemTypeSelectList { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await cases.FindAsync(id.Value);
        if (item is null) return NotFound();

        await SetPermissionsAsync(item);
        if (item.IsDeleted && !UserCan[CaseworkOperation.ManageDeletions])
            return NotFound();

        Item = item;
        NewActionItem = new ActionItemCreateDto(id.Value);
        await PopulateSelectListsAsync();
        return Page();
    }

    /// <summary>
    /// Post is used to add a new Action Item for this Case
    /// </summary>
    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        if (NewActionItem.CaseworkId != id) return BadRequest();

        var item = await cases.FindAsync(id.Value);
        if (item is null) return NotFound();
        if (item.IsDeleted) return BadRequest();

        await SetPermissionsAsync(item);
        if (!UserCan[CaseworkOperation.EditActionItems]) return Forbid();

        if (!ModelState.IsValid)
        {
            Item = item;
            await PopulateSelectListsAsync();
            return Page();
        }

        HighlightId = await actionItems.CreateAsync(NewActionItem);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "New Action successfully added.");
        return RedirectToPage("Details", new { id });
    }

    private async Task PopulateSelectListsAsync() =>
        ActionItemTypeSelectList = (await actionItemTypes.GetListItemsAsync()).ToSelectList();

    private async Task SetPermissionsAsync(CaseworkViewDto item)
    {
        foreach (var operation in CaseworkOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
