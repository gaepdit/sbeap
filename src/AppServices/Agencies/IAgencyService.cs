using GaEpd.AppLibrary.ListItems;

namespace Sbeap.AppServices.Agencies;

public interface IAgencyService : IDisposable
{
    Task<IReadOnlyList<AgencyViewDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetListItemsAsync(bool includeInactive = false, CancellationToken token = default);
    Task<Guid> CreateAsync(AgencyCreateDto resource, CancellationToken token = default);
    Task<AgencyUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(Guid id, AgencyUpdateDto resource, CancellationToken token = default);
}
