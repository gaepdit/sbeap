﻿using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Permissions;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Account;

[Authorize(Policy = PolicyName.ActiveUser)]
public class EditModel : PageModel
{
    // Constructor
    private readonly IStaffService _staffService;
    private readonly IOfficeService _officeService;
    private readonly IValidator<StaffUpdateDto> _validator;

    public EditModel(
        IStaffService staffService,
        IOfficeService officeService,
        IValidator<StaffUpdateDto> validator)
    {
        _staffService = staffService;
        _officeService = officeService;
        _validator = validator;
    }

    // Properties
    [BindProperty]
    public StaffUpdateDto UpdateStaff { get; set; } = default!;

    public StaffViewDto DisplayStaff { get; private set; } = default!;

    // Select lists
    public SelectList OfficeItems { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        var staff = await _staffService.GetCurrentUserAsync();
        if (!staff.Active) return Forbid();

        DisplayStaff = staff;
        UpdateStaff = DisplayStaff.AsUpdateDto();

        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var staff = await _staffService.GetCurrentUserAsync();
        
        // Inactive staff cannot do anything here.
        if (!staff.Active) return Forbid();
        
        // Staff can only update self here.
        if (staff.Id != UpdateStaff.Id) return BadRequest();

        // User cannot deactivate self.
        UpdateStaff.Active = true;

        await _validator.ApplyValidationAsync(UpdateStaff, ModelState);

        if (!ModelState.IsValid)
        {
            DisplayStaff = staff;
            await PopulateSelectListsAsync();
            return Page();
        }

        var result = await _staffService.UpdateAsync(UpdateStaff);
        if (!result.Succeeded) return BadRequest();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Successfully updated profile.");
        return RedirectToPage("Index");
    }

    private async Task PopulateSelectListsAsync() =>
        OfficeItems = (await _officeService.GetActiveListItemsAsync()).ToSelectList();
}
