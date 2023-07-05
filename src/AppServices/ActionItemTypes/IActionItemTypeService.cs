using GaEpd.AppLibrary.ListItems;

namespace Sbeap.AppServices.ActionItemTypes;

public interface IActionItemTypeService : IDisposable
{
    Task<ActionItemTypeViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<ActionItemTypeViewDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default);
    Task<Guid> CreateActionItemTypeAsync(ActionItemTypeCreateDto resource, CancellationToken token = default);
    Task<ActionItemTypeUpdateDto?> FindActionItemTypeForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateActionItemTypeAsync(ActionItemTypeUpdateDto resource, CancellationToken token = default);
    Task DeleteActionItemTypeAsync(Guid id, CancellationToken token = default);
    Task RestoreActionItemTypeAsync(Guid id, CancellationToken token = default);
}
