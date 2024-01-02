using GaEpd.AppLibrary.Extensions;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Permissions;
using Sbeap.Domain.Identity;
using Sbeap.WebApp.Platform.Constants;

namespace Sbeap.WebApp.Pages;

[AllowAnonymous]
public class IndexModel(
    SignInManager<ApplicationUser> signInManager,
    IAuthorizationService authorization,
    ICaseworkService cases)
    : PageModel
{
    // Properties
    public bool ShowDashboard { get; private set; }
    public IPaginatedResult<CaseworkSearchResultDto> OpenCases { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        if (!signInManager.IsSignedIn(User)) return LocalRedirect("~/Account/Login");

        ShowDashboard = await UseDashboardAsync();
        if (!ShowDashboard) return Page();

        // Load dashboard modules
        var openCasesSpec = new CaseworkSearchDto { Status = CaseStatus.Open, Sort = CaseworkSortBy.OpenedDate };
        var paging = new PaginatedRequest(1, GlobalConstants.PageSize, openCasesSpec.Sort.GetDescription());
        OpenCases = await cases.SearchAsync(openCasesSpec, paging);

        return Page();
    }

    private async Task<bool> UseDashboardAsync() =>
        (await authorization.AuthorizeAsync(User, nameof(Policies.StaffUser))).Succeeded;
}
