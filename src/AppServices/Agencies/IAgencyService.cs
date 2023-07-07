using GaEpd.AppLibrary.ListItems;
using Sbeap.AppServices.ActionItemTypes;

namespace Sbeap.AppServices.Agencies;

public interface IAgencyService : IDisposable
{
    Task<AgencyViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<AgencyViewDto>> GetListItemsAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default);
    Task<Guid> CreateAgencyAsync(AgencyCreateDto resource, CancellationToken token = default);
    Task<AgencyUpdateDto?> FindAgencyForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAgencyAsync(AgencyUpdateDto resource, CancellationToken token = default);
    Task DeleteAgencyAsync(Guid id, CancellationToken token = default);
    Task RestoreAgencyAsync(Guid id, CancellationToken token = default);
}
