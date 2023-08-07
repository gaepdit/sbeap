using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Permissions;

namespace Sbeap.WebApp.Pages.Admin.Maintenance.Agency;

[Authorize(Policy = nameof(Policies.AdministrationView))]
public class IndexModel : PageModel
{
    public IReadOnlyList<AgencyViewDto> Items { get; private set; } = default!;

    public static MaintenanceOption ThisOption => MaintenanceOption.Agency;

    public bool IsSiteMaintainer { get; private set; }

    [TempData]
    public Guid? HighlightId { get; set; }

    public async Task OnGetAsync(
        [FromServices] IAgencyService service,
        [FromServices] IAuthorizationService authorization)
    {
        Items = await service.GetListAsync();
        IsSiteMaintainer = (await authorization.AuthorizeAsync(User, nameof(Policies.SiteMaintainer))).Succeeded;
    }
}
