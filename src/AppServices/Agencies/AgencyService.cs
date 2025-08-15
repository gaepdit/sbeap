using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies;

public sealed class AgencyService(
    IAgencyRepository repository,
    IAgencyManager manager,
    IMapper mapper,
    IUserService users,
    IMemoryCache cache) : IAgencyService
{
    private static readonly TimeSpan AgencyListExpiration = TimeSpan.FromDays(7);
    private const string AgencyListCacheKey = nameof(AgencyListCacheKey);

    public async Task<IReadOnlyList<AgencyViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await repository.GetListAsync(token).ConfigureAwait(false)).OrderBy(e => e.Name).ToList();
        return mapper.Map<IReadOnlyList<AgencyViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetListItemsAsync(bool includeInactive = false,
        CancellationToken token = default)
    {
        var key = $"{AgencyListCacheKey}_{includeInactive.ToString()}";
        var list = cache.Get<IReadOnlyList<ListItem>>(key);
        if (list is not null) return list;

        list = (await repository.GetListAsync(e => includeInactive || e.Active, token).ConfigureAwait(false))
            .OrderBy(e => e.Name)
            .Select(e => new ListItem(e.Id, e.NameWithActivity)).ToList();

        cache.Set(key, list, AgencyListExpiration);

        return list;
    }

    public async Task<Guid> CreateAsync(AgencyCreateDto resource, CancellationToken token = default)
    {
        cache.Remove($"{AgencyListCacheKey}_false");
        cache.Remove($"{AgencyListCacheKey}_true");
        var item = await manager
            .CreateAsync(resource.Name, (await users.GetCurrentUserAsync().ConfigureAwait(false))?.Id, token)
            .ConfigureAwait(false);
        await repository.InsertAsync(item, token: token).ConfigureAwait(false);
        return item.Id;
    }

    public async Task<AgencyUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await repository.FindAsync(id, token).ConfigureAwait(false);
        return mapper.Map<AgencyUpdateDto>(item);
    }

    public async Task UpdateAsync(Guid id, AgencyUpdateDto resource, CancellationToken token = default)
    {
        cache.Remove($"{AgencyListCacheKey}_false");
        cache.Remove($"{AgencyListCacheKey}_true");

        var item = await repository.GetAsync(id, token).ConfigureAwait(false);
        item.SetUpdater((await users.GetCurrentUserAsync().ConfigureAwait(false))?.Id);

        if (item.Name != resource.Name.Trim())
            await manager.ChangeNameAsync(item, resource.Name, token).ConfigureAwait(false);
        item.Active = resource.Active;

        await repository.UpdateAsync(item, token: token).ConfigureAwait(false);
    }

    public void Dispose() => repository.Dispose();
    public ValueTask DisposeAsync() => repository.DisposeAsync();
}
