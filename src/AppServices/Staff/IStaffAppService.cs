using Microsoft.AspNetCore.Identity;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.Staff;

public interface IStaffAppService : IDisposable
{
    Task<StaffViewDto?> GetCurrentUserAsync();
    Task<StaffViewDto?> FindAsync(Guid id);
    public Task<List<StaffViewDto>> GetListAsync(StaffSearchDto filter);
    public Task<IList<string>> GetRolesAsync(Guid id);
    public Task<IList<AppRole>> GetAppRolesAsync(Guid id);
    public Task<IdentityResult> UpdateRolesAsync(Guid id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(StaffUpdateDto resource);
}
