using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Mvc;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;

namespace MyAppRoot.WebApp.Api;

[ApiController]
[Route("api/customers")]
[Produces("application/json")]
public class CustomerApiController : Controller
{
    private readonly ICustomerService _service;
    public CustomerApiController(ICustomerService service) => _service = service;

    [HttpGet]
    public async Task<IPaginatedResult<CustomerSearchResultDto>> ListCustomersAsync(
        [FromQuery] CustomerSearchDto spec,
        [FromQuery] ushort page = 1,
        [FromQuery] ushort pageSize = 25) =>
        await _service.SearchAsync(spec, new PaginatedRequest(page, pageSize));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerViewDto>> GetCustomerAsync([FromRoute] Guid id)
    {
        var item = await _service.FindAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }
}
