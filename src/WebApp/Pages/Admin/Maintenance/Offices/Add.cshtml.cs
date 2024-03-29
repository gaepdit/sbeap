﻿using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class AddModel(IOfficeService service, IValidator<OfficeCreateDto> validator)
    : PageModel
{
    // Properties
    [BindProperty]
    public OfficeCreateDto Item { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Office;

    // Methods
    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        HighlightId = await service.CreateAsync(Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully added.");
        return RedirectToPage("Index");
    }
}
