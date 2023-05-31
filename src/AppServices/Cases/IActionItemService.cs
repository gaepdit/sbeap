using Sbeap.AppServices.Cases.Dto;

namespace Sbeap.AppServices.Cases;

public interface IActionItemService : IDisposable
{
    Task<Guid> AddActionItemAsync(ActionItemCreateDto resource, CancellationToken token = default);
    Task<ActionItemUpdateDto?> FindActionItemForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateActionItemAsync(ActionItemUpdateDto resource, CancellationToken token = default);
    Task DeleteActionItemAsync(Guid actionItemId, CancellationToken token = default);
}
