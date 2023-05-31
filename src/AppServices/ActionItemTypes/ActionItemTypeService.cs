using GaEpd.AppLibrary.ListItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Agencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbeap.AppServices.ActionItemTypes;

public sealed class ActionItemTypeService : IActionItemTypeService
{
    private readonly IActionItemTypeRepository _repository;
    
    public ActionItemTypeService(IActionItemTypeRepository repository) => _repository = repository;

    public Task AddActionItemTypeAsync(ActionItemTypeCreateDto resource, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteActionItemTypeAsync(Guid actionItemId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<ActionItemTypeUpdateDto?> FindActionItemTypeForUpdateAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<ListItem>> GetListItemsAsync(bool activeOnly = true, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateActionItemTypeAsync(ActionItemTypeUpdateDto resource, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
