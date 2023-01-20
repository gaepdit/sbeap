﻿using Microsoft.AspNetCore.Identity;
using MyAppRoot.Domain.Identity;

namespace MyAppRoot.AppServices.Staff;

public interface IStaffAppService : IDisposable
{
    Task<StaffViewDto?> GetCurrentUserAsync();
    Task<StaffViewDto?> FindAsync(string id);
    public Task<List<StaffViewDto>> GetListAsync(StaffSearchDto filter);
    public Task<IList<string>> GetRolesAsync(string id);
    public Task<IList<AppRole>> GetAppRolesAsync(string id);
    public Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles);
    Task<IdentityResult> UpdateAsync(StaffUpdateDto resource);
}
