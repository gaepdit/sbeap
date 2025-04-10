using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Offices;

namespace Sbeap.AppServices.Offices;

public sealed class OfficeService(
    IOfficeRepository repository,
    IOfficeManager manager,
    IMapper mapper,
    IUserService users,
    IMemoryCache cache)
    : IOfficeService
{
    private static readonly TimeSpan OfficeListExpiration = TimeSpan.FromDays(7);

    public async Task<IReadOnlyList<OfficeViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await repository.GetListAsync(token: token)).OrderBy(e => e.Name).ToList();
        return mapper.Map<IReadOnlyList<OfficeViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await repository.GetListAsync(e => e.Active, token: token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default)
    {
        var item = await manager.CreateAsync(resource.Name, (await users.GetCurrentUserAsync())?.Id, token);
        await repository.InsertAsync(item, token: token);
        cache.Set(item.Id, item, OfficeListExpiration);
        return item.Id;
    }

    public async Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var office = cache.Get<Office>(id);
        if (office is not null) return mapper.Map<OfficeUpdateDto>(office);

        office = await repository.FindAsync(id, token: token);
        if (office is not null) cache.Set(office.Id, office, OfficeListExpiration);
        return mapper.Map<OfficeUpdateDto>(office);
    }

    public async Task UpdateAsync(Guid id, OfficeUpdateDto resource, CancellationToken token = default)
    {
        var item = await repository.GetAsync(id, token: token);
        item.SetUpdater((await users.GetCurrentUserAsync())?.Id);

        if (item.Name != resource.Name.Trim())
            await manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;

        cache.Set(item.Id, item, OfficeListExpiration);
        await repository.UpdateAsync(item, token: token);
    }

    public void Dispose() => repository.Dispose();
    public async ValueTask DisposeAsync() => await repository.DisposeAsync();
}
