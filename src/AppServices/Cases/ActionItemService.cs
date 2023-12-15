using AutoMapper;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Cases;

namespace Sbeap.AppServices.Cases;

public sealed class ActionItemService(
    IMapper mapper,
    IUserService users,
    ICaseworkRepository cases,
    ICaseworkManager manager,
    IActionItemRepository actionItems,
    IActionItemTypeRepository actionItemTypes)
    : IActionItemService
{
    public async Task<Guid> CreateAsync(ActionItemCreateDto resource, CancellationToken token = default)
    {
        var casework = await cases.GetAsync(resource.CaseworkId, token);
        var actionItemType = await actionItemTypes.GetAsync(resource.ActionItemTypeId!.Value, token);

        var currentUser = await users.GetCurrentUserAsync();
        var item = manager.CreateActionItem(casework, actionItemType, currentUser?.Id);

        item.ActionDate = resource.ActionDate;
        item.Notes = resource.Notes;
        item.EnteredOn = DateTimeOffset.Now;
        item.EnteredBy = currentUser;

        await actionItems.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<ActionItemViewDto?> FindAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ActionItemViewDto>(await actionItems.FindAsync(e => e.Id == id && !e.IsDeleted, token));

    public async Task<ActionItemUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ActionItemUpdateDto>(await actionItems.FindAsync(e => e.Id == id && !e.IsDeleted, token));

    public async Task UpdateAsync(Guid id, ActionItemUpdateDto resource, CancellationToken token = default)
    {
        var item = await actionItems.GetAsync(id, token);
        item.SetUpdater((await users.GetCurrentUserAsync())?.Id);

        item.ActionItemType = await actionItemTypes.GetAsync(resource.ActionItemTypeId!.Value, token);
        item.ActionDate = resource.ActionDate;
        item.Notes = resource.Notes;

        await actionItems.UpdateAsync(item, token: token);
    }

    public async Task DeleteAsync(Guid actionItemId, CancellationToken token = default)
    {
        var item = await actionItems.GetAsync(actionItemId, token);
        item.SetDeleted((await users.GetCurrentUserAsync())?.Id);
        await actionItems.UpdateAsync(item, token: token);
    }

    public void Dispose()
    {
        cases.Dispose();
        actionItems.Dispose();
        actionItemTypes.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await cases.DisposeAsync();
        await actionItems.DisposeAsync();
        await actionItemTypes.DisposeAsync();
    }
}
