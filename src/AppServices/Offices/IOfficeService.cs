using GaEpd.AppLibrary.ListItems;
using Sbeap.AppServices.Staff.Dto;

namespace Sbeap.AppServices.Offices;

public interface IOfficeService : IDisposable
{
    Task<OfficeViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<OfficeViewDto>> GetListAsync(CancellationToken token = default);
    Task<IReadOnlyList<ListItem>> GetActiveListItemsAsync(CancellationToken token = default);
    Task<Guid> CreateAsync(OfficeCreateDto resource, CancellationToken token = default);
    Task<OfficeUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(OfficeUpdateDto resource, CancellationToken token = default);
    Task<IReadOnlyList<StaffViewDto>> GetActiveStaffAsync(Guid id, CancellationToken token = default);
}
