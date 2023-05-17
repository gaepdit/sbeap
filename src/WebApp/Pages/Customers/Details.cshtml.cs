using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.AppServices.Staff;

namespace Sbeap.WebApp.Pages.Customers;

[Authorize(Policy = PolicyName.StaffUser)]
public class DetailsModel : PageModel
{
    // Constructor
    private readonly ICustomerService _customers;
    private readonly IStaffService _staff;
    private readonly IAuthorizationService _authorization;

    public DetailsModel(
        ICustomerService customers,
        IStaffService staff, 
        IAuthorizationService authorization)
    {
        _customers = customers;
        _staff = staff;
        _authorization = authorization;
    }

    // Properties
    public CustomerViewDto Item { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        var staff = await _staff.GetCurrentUserAsync();
        if (staff is not { Active: true }) return Forbid();

        if (id is null) return RedirectToPage("../Index");
        var item = await _customers.FindAsync(id.Value);
        if (item is null) return NotFound();

        Item = item;

        foreach (var operation in CustomerOperation.AllOperations) await SetPermissionAsync(operation);

        if (item.IsDeleted && !UserCan[CustomerOperation.ManageDeletions]) return Forbid();
        return Page();
    }
    private async Task SetPermissionAsync(IAuthorizationRequirement operation) =>
        UserCan[operation] = (await _authorization.AuthorizeAsync(User, Item, operation)).Succeeded;
}
