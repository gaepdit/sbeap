using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.Permissions;

namespace Sbeap.WebApp.Pages.Admin.Maintenance.ActionItemType;

[Authorize(Policy = PolicyName.StaffUser)]
public class IndexModel : PageModel
{
    public IReadOnlyList<ActionItemTypeViewDto> Items { get; private set; } = default!;

    public static MaintenanceOption ThisOption => MaintenanceOption.ActionItemType;

    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync(
        [FromServices] IActionItemTypeService service,
        [FromServices] IAuthorizationService authorization)
    {
        Items = await service.GetListAsync();
        IsSiteMaintainer = (await authorization.AuthorizeAsync(User, PolicyName.SiteMaintainer)).Succeeded;
    }
}
