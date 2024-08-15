using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Admin.Maintenance.Agency;

[Authorize(Policy = nameof(Policies.SiteMaintainer))]
public class AddModel(IAgencyService service, IValidator<AgencyCreateDto> validator)
    : PageModel
{
    // Properties
    [BindProperty]
    public AgencyCreateDto Item { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.Agency;

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
