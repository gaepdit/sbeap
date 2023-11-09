using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.ActionItemTypes;

public sealed class ActionItemTypeService : IActionItemTypeService
{
    private static readonly TimeSpan ActionItemTypeListExpiration = TimeSpan.FromDays(7);
    private const string ActionItemTypeListCacheKey = nameof(ActionItemTypeListCacheKey);

    private readonly IActionItemTypeRepository _repository;
    private readonly IActionItemTypeManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _users;
    private readonly IMemoryCache _cache;

    public ActionItemTypeService(
        IActionItemTypeRepository repository, IActionItemTypeManager manager, IMapper mapper, IUserService users,
        IMemoryCache cache)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _users = users;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ActionItemTypeViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<IReadOnlyList<ActionItemTypeViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetListItemsAsync(CancellationToken token = default)
    {
        var list = _cache.Get<IReadOnlyList<ListItem>>(ActionItemTypeListCacheKey);
        if (list is not null) return list;

        list = (await _repository.GetListAsync(actionItemType => actionItemType.Active, token))
            .OrderBy(actionItemType => actionItemType.Name)
            .Select(actionItemType => new ListItem(actionItemType.Id, actionItemType.Name))
            .ToList();

        _cache.Set(ActionItemTypeListCacheKey, list, ActionItemTypeListExpiration);

        return list;
    }

    public async Task<Guid> CreateAsync(ActionItemTypeCreateDto resource, CancellationToken token = default)
    {
        _cache.Remove(ActionItemTypeListCacheKey);
        var item = await _manager.CreateAsync(resource.Name, (await _users.GetCurrentUserAsync())?.Id, token);
        await _repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<ActionItemTypeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindAsync(id, token);
        return _mapper.Map<ActionItemTypeUpdateDto>(item);
    }

    public async Task UpdateAsync(Guid id, ActionItemTypeUpdateDto resource, CancellationToken token = default)
    {
        _cache.Remove(ActionItemTypeListCacheKey);

        var item = await _repository.GetAsync(id, token);
        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);

        if (item.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;

        await _repository.UpdateAsync(item, token: token);
    }

    public void Dispose() => _repository.Dispose();
    public ValueTask DisposeAsync() => _repository.DisposeAsync();
}
