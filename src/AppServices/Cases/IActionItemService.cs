using Sbeap.AppServices.Cases.Dto;

namespace Sbeap.AppServices.Cases;

public interface IActionItemService : IDisposable
{
    Task<Guid> CreateAsync(ActionItemCreateDto resource, CancellationToken token = default);
    Task<ActionItemViewDto?> FindAsync(Guid actionId, CancellationToken token = default);
    Task<ActionItemUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(ActionItemUpdateDto resource, CancellationToken token = default);
    Task DeleteAsync(Guid actionItemId, CancellationToken token = default);
}
