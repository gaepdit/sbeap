using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Customers;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class DetailsModel : PageModel
{
    // Constructor
    private readonly ICustomerService _customers;
    private readonly ICaseworkService _cases;
    private readonly IAuthorizationService _authorization;

    public DetailsModel(ICustomerService customers, ICaseworkService cases, IAuthorizationService authorization)
    {
        _customers = customers;
        _cases = cases;
        _authorization = authorization;
    }

    // Properties
    [BindProperty]
    public CaseworkCreateDto NewCase { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CustomerViewDto Item { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await _customers.FindAsync(id.Value, await ShowDeletedCasesAsync());
        if (item is null) return NotFound();

        await SetPermissionAsync(item);
        if (item.IsDeleted && !UserCan[CustomerOperation.ManageDeletions])
            return NotFound();

        Item = item;
        NewCase = new CaseworkCreateDto(id.Value);
        return Page();
    }

    /// <summary>
    /// Post is used to add a new Case for this Customer
    /// </summary>
    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await _customers.FindAsync(id.Value);
        if (item is null) return NotFound();
        if (item.IsDeleted) return BadRequest();

        await SetPermissionAsync(item);
        if (!UserCan[CustomerOperation.Edit]) return Forbid();

        if (!ModelState.IsValid)
        {
            Item = item;
            return Page();
        }

        var caseId = await _cases.CreateAsync(NewCase);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "New Case successfully added.");
        return RedirectToPage("../Cases/Details", new { id = caseId });
    }

    private async Task SetPermissionAsync(CustomerViewDto item)
    {
        foreach (var operation in CustomerOperation.AllOperations)
            UserCan[operation] = (await _authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }

    private async Task<bool> ShowDeletedCasesAsync() =>
        (await _authorization.AuthorizeAsync(User, nameof(Policies.AdminUser))).Succeeded;
}
