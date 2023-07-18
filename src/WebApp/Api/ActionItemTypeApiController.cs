﻿using Microsoft.AspNetCore.Mvc;
using Sbeap.AppServices.ActionItemTypes;

namespace MyAppRoot.WebApp.Api;

[ApiController]
[Route("api/ActionItemTypes")]
[Produces("application/json")]
public class ActionItemTypeApiController : Controller
{
    private readonly IActionItemTypeService _actionItemTypesService;
    public ActionItemTypeApiController(IActionItemTypeService actionItemTypeService) => _actionItemTypesService = actionItemTypeService;

    [HttpGet]
    public async Task<IReadOnlyList<ActionItemTypeViewDto>> ListActionItemTypesServiceAsync() =>
        await _actionItemTypesService.GetListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ActionItemTypeViewDto>> GetActionItemTypeAsync([FromRoute] Guid id)
    {
        var item = await _actionItemTypesService.FindAsync(id);
        return item != null ? Ok(item) : Problem("ID not found.", statusCode: 404);
    }
}