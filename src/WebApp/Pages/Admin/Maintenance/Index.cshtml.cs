using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Permissions;

namespace Sbeap.WebApp.Pages.Admin.Maintenance;

[Authorize(Policy = nameof(Policies.AdministrationView))]
public class IndexModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}
