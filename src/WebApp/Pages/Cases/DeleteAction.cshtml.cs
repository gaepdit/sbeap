﻿using Microsoft.AspNetCore.Authorization;
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

    public DeleteActionModel(IActionItemService service, ICaseworkService cases, IAuthorizationService authorization)
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
    public async Task<IActionResult> OnGetAsync(Guid? caseId, Guid? actionId)
    {
        if (caseId is null || actionId is null) return RedirectToPage("Index");
        if (!await UserCanManageDeletionsAsync()) return NotFound();

        var caseView = await _cases.FindBasicInfoAsync(caseId.Value);
        if (caseView is null || caseView.IsDeleted) return NotFound();

        var actionItem = await _service.FindAsync(actionId.Value);
        if (actionItem is null || actionItem.CaseWorkId != caseId) return NotFound();

        CaseView = caseView;
        ActionItemView = actionItem;
        ActionItemId = actionId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!await UserCanManageDeletionsAsync() || !ModelState.IsValid) return BadRequest();

        var originalActionItem = await _service.FindAsync(ActionItemId);
        if (originalActionItem is null) return BadRequest();

        var caseView = await _cases.FindBasicInfoAsync(originalActionItem.CaseWorkId);
        if (caseView is null || caseView.IsDeleted) return BadRequest();

        await _service.DeleteAsync(ActionItemId);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Action Item successfully deleted.");
        return RedirectToPage("Details", new { caseView.Id });
    }

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await _authorization.AuthorizeAsync(User, nameof(Policies.AdminUser))).Succeeded;
}
