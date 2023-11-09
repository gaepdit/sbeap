using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Offices;

namespace Sbeap.AppServices.Offices;

public sealed class OfficeService : IOfficeService
{
    private static readonly TimeSpan OfficeListExpiration = TimeSpan.FromDays(7);

    private readonly IOfficeRepository _repository;
    private readonly IOfficeManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _users;
    private readonly IMemoryCache _cache;

    public OfficeService(
        IOfficeRepository repository, IOfficeManager manager, IMapper mapper, IUserService users, IMemoryCache cache)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _users = users;
        _cache = cache;
    }

    public async Task<IReadOnlyList<OfficeViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<IReadOnlyList<OfficeViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await _repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default)
    {
        var item = await _manager.CreateAsync(resource.Name, (await _users.GetCurrentUserAsync())?.Id, token);
        await _repository.InsertAsync(item, token: token);
        _cache.Set(item.Id, item, OfficeListExpiration);
        return item.Id;
    }

    public async Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var office = _cache.Get<Office>(id);
        if (office is not null) return _mapper.Map<OfficeUpdateDto>(office);

        office = await _repository.FindAsync(id, token);
        if (office is not null) _cache.Set(office.Id, office, OfficeListExpiration);
        return _mapper.Map<OfficeUpdateDto>(office);
    }

    public async Task UpdateAsync(Guid id, OfficeUpdateDto resource, CancellationToken token = default)
    {
        var item = await _repository.GetAsync(id, token);
        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);

        if (item.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;

        _cache.Set(item.Id, item, OfficeListExpiration);
        await _repository.UpdateAsync(item, token: token);
    }

    public void Dispose() => _repository.Dispose();
    public async ValueTask DisposeAsync() => await _repository.DisposeAsync();
}
