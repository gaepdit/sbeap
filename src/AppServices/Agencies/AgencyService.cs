using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies;

public sealed class AgencyService(IAgencyRepository repository, IAgencyManager manager, IMapperBase mapper,
        IUserService users, IMemoryCache cache) : IAgencyService
{
    private static readonly TimeSpan AgencyListExpiration = TimeSpan.FromDays(7);
    private const string AgencyListCacheKey = nameof(AgencyListCacheKey);

    public async Task<IReadOnlyList<AgencyViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return mapper.Map<IReadOnlyList<AgencyViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetListItemsAsync(bool includeInactive = false,
        CancellationToken token = default)
    {
        var key = $"{AgencyListCacheKey}_{includeInactive.ToString()}";
        var list = cache.Get<IReadOnlyList<ListItem>>(key);
        if (list is not null) return list;

        list = (await repository.GetListAsync(e => includeInactive || e.Active, token))
            .OrderBy(e => e.Name)
            .Select(e => new ListItem(e.Id, e.NameWithActivity)).ToList();

        cache.Set(key, list, AgencyListExpiration);

        return list;
    }

    public async Task<Guid> CreateAsync(AgencyCreateDto resource, CancellationToken token = default)
    {
        cache.Remove($"{AgencyListCacheKey}_false");
        cache.Remove($"{AgencyListCacheKey}_true");
        var item = await manager.CreateAsync(resource.Name, (await users.GetCurrentUserAsync())?.Id, token);
        await repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<AgencyUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await repository.FindAsync(id, token);
        return mapper.Map<AgencyUpdateDto>(item);
    }

    public async Task UpdateAsync(Guid id, AgencyUpdateDto resource, CancellationToken token = default)
    {
        cache.Remove($"{AgencyListCacheKey}_false");
        cache.Remove($"{AgencyListCacheKey}_true");

        var item = await repository.GetAsync(id, token);
        item.SetUpdater((await users.GetCurrentUserAsync())?.Id);

        if (item.Name != resource.Name.Trim())
            await manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;

        await repository.UpdateAsync(item, token: token);
    }

    public void Dispose() => repository.Dispose();
    public ValueTask DisposeAsync() => repository.DisposeAsync();
}
