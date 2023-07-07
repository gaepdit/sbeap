using GaEpd.AppLibrary.Enums;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Platform.Constants;

namespace Sbeap.WebApp.Pages.Cases;

[Authorize(Policy = PolicyName.StaffUser)]
public class IndexModel : PageModel
{
    // Constructor
    private readonly ICaseworkService _service;
    private readonly IAgencyService _agencyService;
    private readonly IAuthorizationService _authorization;

    public IndexModel(
        ICaseworkService service,
        IAgencyService agencyService,
        IAuthorizationService authorization)
    {
        _service = service;
        _agencyService = agencyService;
        _authorization = authorization;
    }

    // Properties
    public CaseworkSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<CaseworkSearchResultDto> SearchResults { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();
    public bool ShowDeletionSearchOptions { get; private set; }

    // Select lists
    public SelectList AgencySelectList { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        ShowDeletionSearchOptions = await UserCanManageDeletionsAsync();
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnGetSearchAsync(CaseworkSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();

        ShowDeletionSearchOptions = await UserCanManageDeletionsAsync();
        if (!ShowDeletionSearchOptions) Spec = Spec with { DeletedStatus = null, CustomerDeletedStatus = null };

        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await _service.SearchAsync(Spec, paging);

        ShowResults = true;
        await PopulateSelectListsAsync();
        return Page();
    }

    private async Task PopulateSelectListsAsync() =>
        AgencySelectList = (SelectList)await _agencyService.GetActiveListItemsAsync();

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await _authorization.AuthorizeAsync(User, PolicyName.AdminUser)).Succeeded;
}
