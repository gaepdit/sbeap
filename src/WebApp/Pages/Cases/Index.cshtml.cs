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

    public IndexModel(ICaseworkService service, IAgencyService agencyService)
    {
        _service = service;
        _agencyService = agencyService;
    }

    // Properties
    public CaseworkSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<CaseworkSearchResultDto> SearchResults { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();

    // Select lists
    public SelectList AgencySelectList { get; private set; } = default!;

    // Methods
    public Task OnGetAsync()
    {
        return PopulateSelectListsAsync();
    }

    public async Task<IActionResult> OnGetSearchAsync(CaseworkSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await _service.SearchAsync(Spec, paging);
        ShowResults = true;
        await PopulateSelectListsAsync();
        return Page();
    }

    private async Task PopulateSelectListsAsync() => 
        AgencySelectList = (await _agencyService.GetListItemsAsync(false)).ToSelectList();
}
