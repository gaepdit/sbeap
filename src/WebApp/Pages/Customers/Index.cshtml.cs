using GaEpd.AppLibrary.Enums;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Data;
using Sbeap.WebApp.Platform.Constants;

namespace Sbeap.WebApp.Pages.Customers;

public class IndexModel : PageModel
{
    // Constructor
    private readonly ICustomerService _service;

    public IndexModel(ICustomerService service) => _service = service;

    // Properties
    public CustomerSearchDto Spec { get; set; } = default!;
    public bool ShowResults { get; private set; }
    public IPaginatedResult<CustomerSearchResultDto> SearchResults { get; private set; } = default!;
    public string SortByName => Spec.Sort.ToString();

    // Select lists
    public SelectList CountiesSelectList => new(Data.Counties);

    // Methods
    public void OnGet()
    {
        // Method intentionally left empty.
    }

    public async Task<IActionResult> OnGetSearchAsync(CustomerSearchDto spec, [FromQuery] int p = 1)
    {
        Spec = spec.TrimAll();
        var paging = new PaginatedRequest(p, GlobalConstants.PageSize, Spec.Sort.GetDescription());
        SearchResults = await _service.SearchAsync(Spec, paging);
        ShowResults = true;
        return Page();
    }
}
