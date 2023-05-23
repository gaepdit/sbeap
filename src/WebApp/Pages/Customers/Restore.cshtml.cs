using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Customers;

[Authorize(Policy = PolicyName.AdminUser)]
public class RestoreModel : PageModel
{
    // Constructor
    private readonly ICustomerService _service;
    private readonly IAuthorizationService _authorization;

    public RestoreModel(
        ICustomerService service,
        IAuthorizationService authorization)
    {
        _service = service;
        _authorization = authorization;
    }

    // Properties
    [BindProperty]
    public Guid Id { get; set; }

    public CustomerViewDto Item { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await _service.FindAsync(id.Value);
        if (item is null) return NotFound();

        Id = id.Value;
        Item = item;

        if (!await UserCanManageDeletions()) return Forbid();
        if (Item.IsDeleted) return Page();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Customer is not deleted.");
        return RedirectToPage("Details", new { Item.Id });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var item = await _service.FindAsync(Id);
        if (item is null) return NotFound();

        Item = item;
        if (!await UserCanManageDeletions()) return Forbid();

        if (!Item.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Customer is not deleted.");
            return RedirectToPage("Details", new { Id });
        }

        if (!ModelState.IsValid) return Page();

        await _service.RestoreAsync(Id);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Customer successfully restored.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task<bool> UserCanManageDeletions() =>
        (await _authorization.AuthorizeAsync(User, Item, CustomerOperation.ManageDeletions)).Succeeded;
}
