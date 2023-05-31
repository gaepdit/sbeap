using AutoMapper;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Cases;

namespace Sbeap.AppServices.Cases;

public sealed class ActionItemService : IActionItemService
{
    private readonly IMapper _mapper;
    private readonly IUserService _users;
    private readonly ICaseworkRepository _cases;
    private readonly ICaseworkManager _manager;
    private readonly IActionItemRepository _actionItems;
    private readonly IActionItemTypeRepository _actionItemTypes;

    public ActionItemService(
        IMapper mapper,
        IUserService users,
        ICaseworkRepository cases,
        ICaseworkManager manager,
        IActionItemRepository actionItems,
        IActionItemTypeRepository actionItemTypes)
    {
        _mapper = mapper;
        _users = users;
        _cases = cases;
        _manager = manager;
        _actionItems = actionItems;
        _actionItemTypes = actionItemTypes;
    }

    // Action Items

    public async Task<Guid> AddActionItemAsync(ActionItemCreateDto resource, CancellationToken token = default)
    {
        var casework = await _cases.GetAsync(resource.CaseworkId, token);
        var actionItemType = await _actionItemTypes.GetAsync(resource.ActionItemTypeId!.Value, token);

        var currentUser = await _users.GetCurrentUserAsync();
        var item = _manager.CreateActionItem(casework, actionItemType, currentUser?.Id);

        item.ActionDate = resource.ActionDate;
        item.Notes = resource.Notes;
        item.EnteredOn = DateTimeOffset.Now;
        item.EnteredBy = currentUser;

        await _actionItems.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<ActionItemUpdateDto?> FindActionItemForUpdateAsync(Guid id, CancellationToken token = default) =>
        _mapper.Map<ActionItemUpdateDto>(await _actionItems.FindAsync(id, token));

    public async Task UpdateActionItemAsync(ActionItemUpdateDto resource, CancellationToken token = default)
    {
        var item = await _actionItems.GetAsync(resource.Id, token);
        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);

        item.ActionItemType = await _actionItemTypes.GetAsync(resource.ActionItemTypeId!.Value, token);
        item.ActionDate = resource.ActionDate;
        item.Notes = resource.Notes;

        await _actionItems.UpdateAsync(item, token: token);
    }

    public async Task DeleteActionItemAsync(Guid actionItemId, CancellationToken token = default)
    {
        var item = await _actionItems.GetAsync(actionItemId, token);
        item.SetDeleted((await _users.GetCurrentUserAsync())?.Id);
        await _actionItems.UpdateAsync(item, token: token);
    }

    public void Dispose()
    {
        _cases.Dispose();
        _actionItems.Dispose();
        _actionItemTypes.Dispose();
    }
}
