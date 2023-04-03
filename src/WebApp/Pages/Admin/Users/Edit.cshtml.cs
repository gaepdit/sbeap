﻿using FluentValidation;
using FluentValidation.AspNetCore;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Permissions;
using Sbeap.AppServices.Staff;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.RazorHelpers;

namespace Sbeap.WebApp.Pages.Admin.Users;

[Authorize(Policy = PolicyName.UserAdministrator)]
public class EditModel : PageModel
{
    private readonly IStaffAppService _staffService;
    private readonly IOfficeAppService _officeService;
    private readonly IValidator<StaffUpdateDto> _validator;

    public EditModel(
        IStaffAppService staffService,
        IOfficeAppService officeService,
        IValidator<StaffUpdateDto> validator)
    {
        _staffService = staffService;
        _officeService = officeService;
        _validator = validator;
    }

    [BindProperty]
    public StaffUpdateDto UpdateStaff { get; set; } = default!;

    public StaffViewDto DisplayStaff { get; private set; } = default!;
    public SelectList OfficeItems { get; private set; } = default!;

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id is null) return RedirectToPage("Index");
        var staff = await _staffService.FindAsync(id);
        if (staff is null) return NotFound();

        DisplayStaff = staff;
        UpdateStaff = DisplayStaff.AsUpdateDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var validationResult = await _validator.ValidateAsync(UpdateStaff);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(UpdateStaff));

        if (!ModelState.IsValid)
        {
            var staff = await _staffService.FindAsync(UpdateStaff.Id);
            if (staff is null) return BadRequest();

            DisplayStaff = staff;

            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await _staffService.UpdateAsync(UpdateStaff);
        if (!result.Succeeded) return BadRequest();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated.");
        return RedirectToPage("Details", new { id = UpdateStaff.Id });
    }

    private async Task PopulateSelectListsAsync() =>
        OfficeItems = (await _officeService.GetActiveListItemsAsync()).ToSelectList();
}
