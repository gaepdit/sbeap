using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Mvc;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;

namespace MyAppRoot.WebApp.Api;

[ApiController]
[Route("api/cases")]
[Produces("application/json")]
public class CaseworkApiController : Controller
{
    private readonly ICaseworkService _service;
    public CaseworkApiController(ICaseworkService service) => _service = service;

    [HttpGet]
    public async Task<IPaginatedResult<CaseworkSearchResultDto>> ListCasesAsync(
        [FromQuery] CaseworkSearchDto spec,
        [FromQuery] ushort page = 1,
        [FromQuery] ushort pageSize = 25) =>
        await _service.SearchAsync(spec, new PaginatedRequest(page, pageSize));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CaseworkViewDto>> GetCaseworkAsync([FromRoute] Guid id)
    {
        var item = await _service.FindAsync(id);
        return item is null ? Problem("ID not found.", statusCode: 404) : Ok(item);
    }
}
