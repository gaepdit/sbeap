﻿using GaEpd.AppLibrary.Enums;
using GaEpd.AppLibrary.ListItems;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.Domain.Identity;
using Sbeap.WebApp.Platform.Constants;

namespace Sbeap.WebApp.Pages.Admin.Users;

[Authorize]
public class IndexModel : PageModel
{
    // Constructor
    private readonly IOfficeAppService _officeService;
    private readonly IStaffAppService _staffService;

    public IndexModel(
        IOfficeAppService officeService,
        IStaffAppService staffService)
    {
        _officeService = officeService;
        _staffService = staffService;
    }

    // Properties
    public StaffSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<StaffSearchResultDto> SearchResults { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();
    
    [TempData]
    public string? HighlightId { get; set; }

    // Select lists
    public SelectList RoleItems { get; private set; } = default!;
    public SelectList OfficeItems { get; private set; } = default!;

    // Methods
    public Task OnGetAsync() => PopulateSelectListsAsync();

    public async Task<IActionResult> OnGetSearchAsync(StaffSearchDto spec, [FromQuery] int p = 1)
    {
        spec.TrimAll();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, spec.Sort.GetDescription());

        Spec = spec;
        ShowResults = true;

        await PopulateSelectListsAsync();
        SearchResults = await _staffService.SearchAsync(spec, paging);
        return Page();
    }

    private async Task PopulateSelectListsAsync()
    {
        OfficeItems = (await _officeService.GetActiveListItemsAsync()).ToSelectList();
        RoleItems = AppRole.AllRoles
            .Select(r => new ListItem<string>(r.Key, r.Value.DisplayName))
            .ToSelectList();
    }
}
