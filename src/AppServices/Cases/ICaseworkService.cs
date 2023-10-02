using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Cases.Dto;

namespace Sbeap.AppServices.Cases;

public interface ICaseworkService : IDisposable
{
    // Casework read
    Task<IPaginatedResult<CaseworkSearchResultDto>> SearchAsync(
        CaseworkSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    Task<CaseworkViewDto?> FindAsync(Guid id, CancellationToken token = default);
    Task<CaseworkSearchResultDto?> FindBasicInfoAsync(Guid id, CancellationToken token = default);

    // Casework write
    Task<Guid> CreateAsync(CaseworkCreateDto resource, CancellationToken token = default);
    Task<CaseworkUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(Guid id, CaseworkUpdateDto resource, CancellationToken token = default);
    Task DeleteAsync(Guid id, string? deleteComments, CancellationToken token = default);
    Task RestoreAsync(Guid id, CancellationToken token = default);
}
