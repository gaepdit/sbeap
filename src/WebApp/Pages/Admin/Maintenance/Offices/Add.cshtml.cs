﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Offices;
using Sbeap.Domain.Identity;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.RazorHelpers;

namespace Sbeap.WebApp.Pages.Admin.Maintenance.Offices;

[Authorize(Roles = AppRole.SiteMaintenance)]
public class AddModel : PageModel
{
    [BindProperty]
    public OfficeCreateDto Item { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Office;

    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnPostAsync(
        [FromServices] IOfficeAppService service,
        [FromServices] IValidator<OfficeCreateDto> validator)
    {
        var validationResult = await validator.ValidateAsync(Item);
        if (!validationResult.IsValid) validationResult.AddToModelState(ModelState, nameof(Item));
        if (!ModelState.IsValid) return Page();

        var id = await service.CreateAsync(Item);

        HighlightId = id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully added.");
        return RedirectToPage("Index");
    }
}
