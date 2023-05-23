using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.AppServices.Staff;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Customers;

[Authorize(Policy = PolicyName.StaffUser)]
public class DetailsModel : PageModel
{
    // Constructor
    private readonly ICustomerService _customers;
    private readonly ICaseworkService _cases;
    private readonly IStaffService _staff;
    private readonly IAuthorizationService _authorization;

    public DetailsModel(
        ICustomerService customers,
        ICaseworkService cases,
        IStaffService staff,
        IAuthorizationService authorization)
    {
        _customers = customers;
        _cases = cases;
        _staff = staff;
        _authorization = authorization;
    }

    // Properties
    public CustomerViewDto Item { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    [BindProperty]
    public CaseworkCreateDto NewCase { get; set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (await _staff.GetCurrentUserAsync() is not { Active: true }) return Forbid();

        if (id is null) return RedirectToPage("../Index");
        var item = await _customers.FindAsync(id.Value, await ShowDeletedCasesAsync());
        if (item is null) return NotFound();

        Item = item;

        foreach (var operation in CustomerOperation.AllOperations) await SetPermissionAsync(operation);
        if (Item.IsDeleted && !UserCan[CustomerOperation.ManageDeletions]) return Forbid();

        NewCase = new CaseworkCreateDto(id.Value);
        return Page();
    }

    /// <summary>
    /// Post is used to add a new Case for this Customer
    /// </summary>
    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await _customers.FindAsync(id.Value, await ShowDeletedCasesAsync());
        if (item is null) return NotFound();

        Item = item;

        foreach (var operation in CustomerOperation.AllOperations) await SetPermissionAsync(operation);
        if (!UserCan[CustomerOperation.Edit]) return Forbid();

        if (!ModelState.IsValid) return Page();

        var caseId = await _cases.CreateAsync(NewCase);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "New Case successfully added.");
        return RedirectToPage("../Cases/Details", new { id = caseId });
    }

    private async Task SetPermissionAsync(IAuthorizationRequirement operation) =>
        UserCan[operation] = (await _authorization.AuthorizeAsync(User, Item, operation)).Succeeded;

    private async Task<bool> ShowDeletedCasesAsync() =>
        (await _authorization.AuthorizeAsync(User, PolicyName.AdminUser)).Succeeded;
}
