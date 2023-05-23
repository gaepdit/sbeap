using GaEpd.AppLibrary.Enums;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.Domain.Identity;
using Sbeap.WebApp.Platform.Constants;

namespace Sbeap.WebApp.Pages;

[AllowAnonymous]
public class IndexModel : PageModel
{
    // Constructor
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ICaseworkService _cases;

    public IndexModel(SignInManager<ApplicationUser> signInManager, ICaseworkService cases)
    {
        _signInManager = signInManager;
        _cases = cases;
    }

    // Properties
    public bool ShowDashboard { get; private set; }
    public IPaginatedResult<CaseworkSearchResultDto> OpenCases { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        if (!_signInManager.IsSignedIn(User)) return LocalRedirect("~/Account/Login");
        if (!User.IsInRole(RoleName.Staff) && !User.IsInRole(RoleName.Admin)) return Page();

        var openCasesSpec = new CaseworkSearchDto { Status = CaseStatus.Open, Sort = CaseworkSortBy.OpenedDate };
        var paging = new PaginatedRequest(1, GlobalConstants.PageSize, openCasesSpec.Sort.GetDescription());
        OpenCases = await _cases.SearchAsync(openCasesSpec, paging);

        ShowDashboard = true;
        return Page();
    }
}
