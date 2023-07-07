using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Admin.Maintenance.Agency;

[Authorize(Policy = PolicyName.SiteMaintainer)]
public class AddModel : PageModel
{
    // Constructor
    private readonly IAgencyService _service;
    private readonly IValidator<AgencyCreateDto> _validator;

    public AddModel(
        IAgencyService service,
        IValidator<AgencyCreateDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    // Properties
    [BindProperty]
    public AgencyCreateDto Item { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public static MaintenanceOption ThisOption => MaintenanceOption.ActionItemType;

    // Methods
    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        HighlightId = await _service.CreateAgencyAsync(Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, $"“{Item.Name}” successfully added.");
        return RedirectToPage("Index");
    }
}

