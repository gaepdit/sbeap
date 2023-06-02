using GaEpd.AppLibrary.ListItems;
using Sbeap.AppServices.Cases.Dto;

namespace Sbeap.AppServices.ActionItemTypes;

public interface IActionItemTypeService : IDisposable
{
    Task<IReadOnlyList<ListItem>> GetListItemsAsync(bool activeOnly = true, CancellationToken token = default);
    Task<Guid> CreateActionItemTypeAsync(ActionItemTypeCreateDto resource, CancellationToken token = default);
    Task<ActionItemTypeUpdateDto?> FindActionItemTypeForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateActionItemTypeAsync(ActionItemTypeUpdateDto resource, CancellationToken token = default);
    Task DeleteActionItemTypeAsync(Guid id, CancellationToken token = default);
    Task RestoreActionItemTypeAsync(Guid id, CancellationToken token = default);
}
