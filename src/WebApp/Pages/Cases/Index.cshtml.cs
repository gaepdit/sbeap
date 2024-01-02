using GaEpd.AppLibrary.Extensions;
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
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.Constants;

namespace Sbeap.WebApp.Pages.Cases;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class IndexModel(ICaseworkService service, IAgencyService agencyService, IAuthorizationService authorization)
    : PageModel
{
    // Properties
    public CaseworkSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<CaseworkSearchResultDto> SearchResults { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();
    public bool ShowDeletionSearchOptions { get; private set; }
    public PaginationNavModel PaginationNav => new(SearchResults, Spec.AsRouteValues());

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
        SearchResults = await service.SearchAsync(Spec, paging);

        ShowResults = true;
        await PopulateSelectListsAsync();
        return Page();
    }

    private async Task PopulateSelectListsAsync() =>
        AgencySelectList = (await agencyService.GetListItemsAsync(true)).ToSelectList();

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await authorization.AuthorizeAsync(User, nameof(Policies.AdminUser))).Succeeded;
}
