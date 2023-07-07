﻿using Microsoft.AspNetCore.Mvc;
using Sbeap.AppServices.Agencies;

namespace Sbeap.WebApp.Api;

[ApiController]
[Route("api/Agency")]
[Produces("application/json")]
public class AgencyApiController : Controller
{
    private readonly IAgencyService _agencyService;
    public AgencyApiController(IAgencyService agencyService) => _agencyService = agencyService;

    [HttpGet]
    public async Task<IReadOnlyList<AgencyViewDto>> ListAgencyServiceAsync() =>
        await _agencyService.GetListItemsAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AgencyViewDto>> GetActionItemTypeAsync([FromRoute] Guid id)
    {
        var item = await _agencyService.FindAgencyForUpdateAsync(id);
        return item != null ? Ok(item) : Problem("ID not found.", statusCode: 404);
    }
}
