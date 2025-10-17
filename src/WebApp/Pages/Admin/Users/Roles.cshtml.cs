using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.AuthorizationPolicies;

namespace Sbeap.WebApp.Pages.Admin.Users;

[Authorize(Policy = nameof(Policies.ActiveUser))]
public class RolesModel : PageModel
{
    public void OnGet()
    {
        // Method intentionally left empty.
    }
}
