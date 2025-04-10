using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.ActionItemTypes;

public sealed class ActionItemTypeService(
    IActionItemTypeRepository repository,
    IActionItemTypeManager manager,
    IMapper mapper,
    IUserService users,
    IMemoryCache cache) : IActionItemTypeService
{
    private static readonly TimeSpan ActionItemTypeListExpiration = TimeSpan.FromDays(7);
    private const string ActionItemTypeListCacheKey = nameof(ActionItemTypeListCacheKey);

    public async Task<IReadOnlyList<ActionItemTypeViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await repository.GetListAsync(token: token)).OrderBy(e => e.Name).ToList();
        return mapper.Map<IReadOnlyList<ActionItemTypeViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default)
    {
        var list = cache.Get<IReadOnlyList<ListItem>>(ActionItemTypeListCacheKey);
        if (list is not null) return list;

        list = (await repository.GetListAsync(actionItemType => actionItemType.Active, token: token))
            .OrderBy(actionItemType => actionItemType.Name)
            .Select(actionItemType => new ListItem(actionItemType.Id, actionItemType.Name))
            .ToList();

        cache.Set(ActionItemTypeListCacheKey, list, ActionItemTypeListExpiration);

        return list;
    }

    public async Task<Guid> CreateAsync(ActionItemTypeCreateDto resource, CancellationToken token = default)
    {
        cache.Remove(ActionItemTypeListCacheKey);
        var item = await manager.CreateAsync(resource.Name, (await users.GetCurrentUserAsync())?.Id, token);
        await repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<ActionItemTypeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await repository.FindAsync(id, token: token);
        return mapper.Map<ActionItemTypeUpdateDto>(item);
    }

    public async Task UpdateAsync(Guid id, ActionItemTypeUpdateDto resource, CancellationToken token = default)
    {
        cache.Remove(ActionItemTypeListCacheKey);

        var item = await repository.GetAsync(id, token: token);
        item.SetUpdater((await users.GetCurrentUserAsync())?.Id);

        if (item.Name != resource.Name.Trim())
            await manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;

        await repository.UpdateAsync(item, token: token);
    }

    public void Dispose() => repository.Dispose();
    public ValueTask DisposeAsync() => repository.DisposeAsync();
}
