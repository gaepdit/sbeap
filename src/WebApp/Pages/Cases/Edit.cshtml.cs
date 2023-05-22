﻿using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Cases;

[Authorize(Policy = PolicyName.StaffUser)]
public class EditModel : PageModel
{
    private readonly ICaseworkService _service;
    private readonly IAgencyService _agencyService;
    private readonly IValidator<CaseworkUpdateDto> _validator;
    private readonly IAuthorizationService _authorization;

    public EditModel(
        ICaseworkService service,
        IAgencyService agencyService,
        IValidator<CaseworkUpdateDto> validator, 
        IAuthorizationService authorization)
    {
        _service = service;
        _agencyService = agencyService;
        _validator = validator;
        _authorization = authorization;
    }

    // Properties
    [BindProperty]
    public CaseworkUpdateDto Item { get; set; } = default!;

    // Select lists
    public SelectList AgencySelectList { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await _service.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();

        Item = item;
        
        if (!await UserCanEdit()) return Forbid();
        
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!await UserCanEdit()) return Forbid();
        await _validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await _service.UpdateAsync(Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Case successfully updated.");
        return RedirectToPage("Details", new { Item.Id });
    }

    private async Task PopulateSelectListsAsync() =>
        AgencySelectList = (await _agencyService.GetListItemsAsync()).ToSelectList();

    private async Task<bool> UserCanEdit() =>
        (await _authorization.AuthorizeAsync(User, Item, CaseworkOperation.Edit)).Succeeded;
}