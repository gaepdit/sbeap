using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies;

public sealed class AgencyService : IAgencyService
{
    private readonly IAgencyRepository _repository;
    private readonly IAgencyManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _users;
    public AgencyService(
        IAgencyRepository repository,
        IAgencyManager manager,
        IMapper mapper,
        IUserService users)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _users = users;
    }

    public async Task<AgencyViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindAsync(id, token);
        return _mapper.Map<AgencyViewDto>(item);
    }

    public async Task<IReadOnlyList<AgencyViewDto>> GetListAsync(bool active = true, CancellationToken token = default)
    {
        var list = (await _repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<IReadOnlyList<AgencyViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetListItemsAsync(bool active = true, CancellationToken token = default) =>
        (await _repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateAsync(AgencyCreateDto resource, CancellationToken token = default)
    {
        var item = await _manager.CreateAsync(resource.Name, (await _users.GetCurrentUserAsync())?.Id, token);
        await _repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<AgencyUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindAsync(id, token);
        return _mapper.Map<AgencyUpdateDto>(item);
    }

    public async Task UpdateAsync(AgencyUpdateDto resource, CancellationToken token = default)
    {
        var item = await _repository.GetAsync(resource.Id, token);
        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);

        if (item.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;

        await _repository.UpdateAsync(item, token: token);
    }

    public void Dispose() => _repository.Dispose();
}
