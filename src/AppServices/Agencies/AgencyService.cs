using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies;

public sealed class AgencyService : IAgencyService
{
    private static readonly TimeSpan AgencyListExpiration = TimeSpan.FromDays(7);
    private const string AgencyListCacheKey = nameof(AgencyListCacheKey);

    private readonly IAgencyRepository _repository;
    private readonly IAgencyManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _users;
    private readonly IMemoryCache _cache;

    public AgencyService(
        IAgencyRepository repository, IAgencyManager manager, IMapper mapper, IUserService users, IMemoryCache cache)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _users = users;
        _cache = cache;
    }

    public async Task<IReadOnlyList<AgencyViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<IReadOnlyList<AgencyViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetListItemsAsync(bool includeInactive = false,
        CancellationToken token = default)
    {
        var key = $"{AgencyListCacheKey}_{includeInactive.ToString()}";
        var list = _cache.Get<IReadOnlyList<ListItem>>(key);
        if (list is not null) return list;

        list = (await _repository.GetListAsync(e => includeInactive || e.Active, token))
            .OrderBy(e => e.Name)
            .Select(e => new ListItem(e.Id, e.NameWithActivity)).ToList();

        _cache.Set(key, list, AgencyListExpiration);

        return list;
    }

    public async Task<Guid> CreateAsync(AgencyCreateDto resource, CancellationToken token = default)
    {
        _cache.Remove($"{AgencyListCacheKey}_false");
        _cache.Remove($"{AgencyListCacheKey}_true");
        var item = await _manager.CreateAsync(resource.Name, (await _users.GetCurrentUserAsync())?.Id, token);
        await _repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<AgencyUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindAsync(id, token);
        return _mapper.Map<AgencyUpdateDto>(item);
    }

    public async Task UpdateAsync(Guid id, AgencyUpdateDto resource, CancellationToken token = default)
    {
        _cache.Remove($"{AgencyListCacheKey}_false");
        _cache.Remove($"{AgencyListCacheKey}_true");

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
