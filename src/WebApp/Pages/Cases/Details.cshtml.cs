using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.AppServices.Permissions;

namespace Sbeap.WebApp.Pages.Cases;

[Authorize(Policy = PolicyName.StaffUser)]
public class DetailsModel : PageModel
{
    // Constructor
    private readonly ICaseworkService _cases;
    private readonly IAuthorizationService _authorization;

    public DetailsModel(
        ICaseworkService cases,
        IAuthorizationService authorization)
    {
        _cases = cases;
        _authorization = authorization;
    }

    // Properties
    public CaseworkViewDto Item { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var item = await _cases.FindAsync(id.Value);
        if (item is null) return NotFound();

        Item = item;

        foreach (var operation in CaseworkOperation.AllOperations) await SetPermissionAsync(operation);

        if (item.IsDeleted && !UserCan[CaseworkOperation.ManageDeletions]) return Forbid();
        return Page();
    }

    private async Task SetPermissionAsync(IAuthorizationRequirement operation) =>
        UserCan[operation] = (await _authorization.AuthorizeAsync(User, Item, operation)).Succeeded;
}
