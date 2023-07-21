using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Cases;

[Authorize(Policy = nameof(Policies.AdminUser))]
public class DeleteActionModel : PageModel
{
    // Constructor
    private readonly IActionItemService _service;
    private readonly ICaseworkService _cases;
    private readonly IAuthorizationService _authorization;

    public DeleteActionModel(
        IActionItemService service,
        ICaseworkService cases,
        IAuthorizationService authorization)
    {
        _service = service;
        _cases = cases;
        _authorization = authorization;
    }

    // Properties
    [BindProperty]
    public Guid ActionItemId { get; set; }

    public ActionItemViewDto ActionItemView { get; private set; } = default!;
    public CaseworkSearchResultDto CaseView { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id, Guid? actionId)
    {
        if (id is null || actionId is null) return RedirectToPage("Index");

        var casework = await _cases.FindBasicInfoAsync(id.Value);
        if (casework is null) return NotFound();
        CaseView = casework;

        if (CaseView.IsDeleted || !await UserCanManageDeletionsAsync())
            return NotFound();

        var actionItem = await _service.FindAsync(actionId.Value);
        if (actionItem is null) return NotFound();
        ActionItemView = actionItem;

        ActionItemId = actionId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");

        var casework = await _cases.FindBasicInfoAsync(id.Value);
        if (casework is null) return BadRequest();
        CaseView = casework;

        if (CaseView.IsDeleted || !await UserCanManageDeletionsAsync() || !ModelState.IsValid)
            return BadRequest();

        await _service.DeleteAsync(ActionItemId);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Action Item successfully deleted.");
        return RedirectToPage("Details", new { id });
    }

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await _authorization.AuthorizeAsync(User, nameof(Policies.AdminUser))).Succeeded;
}
