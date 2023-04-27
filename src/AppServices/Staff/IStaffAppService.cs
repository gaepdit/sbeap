using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Identity;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.Staff;

public interface IStaffAppService : IDisposable
{
    Task<StaffViewDto> GetCurrentUserAsync();
    Task<StaffViewDto?> FindAsync(string id);
    Task<List<StaffViewDto>> GetListAsync(StaffSearchDto spec);

    Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(
        StaffSearchDto spec, PaginatedRequest paging, CancellationToken token = default);

    Task<IList<string>> GetRolesAsync(string id);
    Task<IList<AppRole>> GetAppRolesAsync(string id);
    Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(StaffUpdateDto resource);
}
