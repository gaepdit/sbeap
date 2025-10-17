using AutoMapper;
using Sbeap.AppServices.AuthenticationServices;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Cases;

namespace Sbeap.AppServices.Cases;

public sealed class ActionItemService(
    IMapper mapper,
    IUserService userService,
    ICaseworkRepository caseworkRepository,
    ICaseworkManager caseworkManager,
    IActionItemRepository actionItemRepository,
    IActionItemTypeRepository actionItemTypeRepository)
    : IActionItemService
{
    public async Task<Guid> CreateAsync(ActionItemCreateDto resource, CancellationToken token = default)
    {
        var casework = await caseworkRepository.GetAsync(resource.CaseworkId, token).ConfigureAwait(false);
        var actionItemType = await actionItemTypeRepository.GetAsync(resource.ActionItemTypeId!.Value, token)
            .ConfigureAwait(false);

        var currentUser = await userService.GetCurrentUserAsync().ConfigureAwait(false);
        var item = caseworkManager.CreateActionItem(casework, actionItemType, currentUser?.Id);

        item.ActionDate = resource.ActionDate;
        item.Notes = resource.Notes;
        item.EnteredOn = DateTimeOffset.Now;
        item.EnteredBy = currentUser;

        await actionItemRepository.InsertAsync(item, token: token).ConfigureAwait(false);
        return item.Id;
    }

    public async Task<ActionItemViewDto?> FindAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ActionItemViewDto>(await actionItemRepository.FindAsync(e => e.Id == id && !e.IsDeleted, token)
            .ConfigureAwait(false));

    public async Task<ActionItemUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<ActionItemUpdateDto>(await actionItemRepository.FindAsync(e => e.Id == id && !e.IsDeleted, token)
            .ConfigureAwait(false));

    public async Task UpdateAsync(Guid id, ActionItemUpdateDto resource, CancellationToken token = default)
    {
        var item = await actionItemRepository.GetAsync(id, token).ConfigureAwait(false);
        item.SetUpdater((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        item.ActionItemType = await actionItemTypeRepository.GetAsync(resource.ActionItemTypeId!.Value, token)
            .ConfigureAwait(false);
        item.ActionDate = resource.ActionDate;
        item.Notes = resource.Notes;

        await actionItemRepository.UpdateAsync(item, token: token).ConfigureAwait(false);
    }

    public async Task DeleteAsync(Guid actionItemId, CancellationToken token = default)
    {
        var item = await actionItemRepository.GetAsync(actionItemId, token).ConfigureAwait(false);
        item.SetDeleted((await userService.GetCurrentUserAsync().ConfigureAwait(false))?.Id);
        await actionItemRepository.UpdateAsync(item, token: token).ConfigureAwait(false);
    }

    public void Dispose()
    {
        caseworkRepository.Dispose();
        actionItemRepository.Dispose();
        actionItemTypeRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await caseworkRepository.DisposeAsync().ConfigureAwait(false);
        await actionItemRepository.DisposeAsync().ConfigureAwait(false);
        await actionItemTypeRepository.DisposeAsync().ConfigureAwait(false);
    }
}
