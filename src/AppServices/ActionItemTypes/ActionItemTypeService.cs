using AutoMapper;
using GaEpd.AppLibrary.ListItems;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.ActionItemTypes;

public sealed class ActionItemTypeService : IActionItemTypeService
{
    private readonly IActionItemTypeRepository _repository;
    private readonly IActionItemTypeManager _manager;
    private readonly IMapper _mapper;
    private readonly IUserService _users;
    public ActionItemTypeService(
        IActionItemTypeRepository repository,
        IActionItemTypeManager manager,
        IMapper mapper,
        IUserService users)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _users = users;
    }

    public async Task<ActionItemTypeViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindAsync(id, token);
        return _mapper.Map<ActionItemTypeViewDto>(item);
    }

    public async Task<IReadOnlyList<ActionItemTypeViewDto>> GetListAsync(CancellationToken token = default)
    {
        var list = (await _repository.GetListAsync(token)).OrderBy(e => e.Name).ToList();
        return _mapper.Map<IReadOnlyList<ActionItemTypeViewDto>>(list);
    }

    public async Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await _repository.GetListAsync(e => e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.Name)).ToList();

    public async Task<Guid> CreateActionItemTypeAsync(ActionItemTypeCreateDto resource, CancellationToken token = default)
    {
        var item = await _manager.CreateAsync(resource.Name, (await _users.GetCurrentUserAsync())?.Id, token);
        await _repository.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<ActionItemTypeUpdateDto?> FindActionItemTypeForUpdateAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.FindAsync(id, token);
        return _mapper.Map<ActionItemTypeUpdateDto>(item);
    }
    public async Task UpdateActionItemTypeAsync(ActionItemTypeUpdateDto resource, CancellationToken token = default)
    {
        var item = await _repository.GetAsync(resource.Id, token);
        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);

        if (item.Name != resource.Name.Trim())
            await _manager.ChangeNameAsync(item, resource.Name, token);
        item.Active = resource.Active;

        await _repository.UpdateAsync(item, token: token);
    }
    public async Task DeleteActionItemTypeAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.GetAsync(id, token);
        item.Active = false;
        await _repository.UpdateAsync(item, token: token);
    }

    public async Task RestoreActionItemTypeAsync(Guid id, CancellationToken token = default)
    {
        var item = await _repository.GetAsync(id, token);
        item.Active = true;
        await _repository.UpdateAsync(item, token: token);
    }

    public void Dispose() => _repository.Dispose();
}
