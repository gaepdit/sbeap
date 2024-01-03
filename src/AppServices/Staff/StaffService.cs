﻿using AutoMapper;
using GaEpd.AppLibrary.Domain.Repositories;
using GaEpd.AppLibrary.Pagination;
using Microsoft.AspNetCore.Identity;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.Staff;

public sealed class StaffService(
    IUserService userService,
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    IOfficeRepository officeRepository)
    : IStaffService
{
    public async Task<StaffViewDto> GetCurrentUserAsync()
    {
        var user = await userService.GetCurrentUserAsync()
            ?? throw new CurrentUserNotFoundException();
        return mapper.Map<StaffViewDto>(user);
    }

    public async Task<StaffViewDto?> FindAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        return mapper.Map<StaffViewDto?>(user);
    }

    public async Task<List<StaffViewDto>> GetListAsync(StaffSearchDto spec)
    {
        var users = string.IsNullOrEmpty(spec.Role)
            ? userManager.Users.ApplyFilter(spec)
            : (await userManager.GetUsersInRoleAsync(spec.Role)).AsQueryable().ApplyFilter(spec);

        return mapper.Map<List<StaffViewDto>>(users);
    }

    public async Task<IPaginatedResult<StaffSearchResultDto>> SearchAsync(StaffSearchDto spec, PaginatedRequest paging)
    {
        var users = string.IsNullOrEmpty(spec.Role)
            ? userManager.Users.ApplyFilter(spec)
            : (await userManager.GetUsersInRoleAsync(spec.Role)).AsQueryable().ApplyFilter(spec);
        var list = users.Skip(paging.Skip).Take(paging.Take);
        var listMapped = mapper.Map<List<StaffSearchResultDto>>(list);

        return new PaginatedResult<StaffSearchResultDto>(listMapped, users.Count(), paging);
    }

    public async Task<IList<string>> GetRolesAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null) return new List<string>();
        return await userManager.GetRolesAsync(user);
    }

    public async Task<IList<AppRole>> GetAppRolesAsync(string id) =>
        AppRole.RolesAsAppRoles(await GetRolesAsync(id)).OrderBy(r => r.DisplayName).ToList();

    public async Task<IdentityResult> UpdateRolesAsync(string id, Dictionary<string, bool> roles)
    {
        var user = await userManager.FindByIdAsync(id)
            ?? throw new EntityNotFoundException<ApplicationUser>(id);

        foreach (var (role, value) in roles)
        {
            var result = await UpdateUserRoleAsync(user, role, value);
            if (result != IdentityResult.Success) return result;
        }

        return IdentityResult.Success;

        async Task<IdentityResult> UpdateUserRoleAsync(ApplicationUser u, string r, bool addToRole)
        {
            var isInRole = await userManager.IsInRoleAsync(u, r);
            if (addToRole == isInRole) return IdentityResult.Success;

            return addToRole switch
            {
                true => await userManager.AddToRoleAsync(u, r),
                false => await userManager.RemoveFromRoleAsync(u, r),
            };
        }
    }

    public async Task<IdentityResult> UpdateAsync(string id, StaffUpdateDto resource)
    {
        var user = await userManager.FindByIdAsync(id)
            ?? throw new EntityNotFoundException<ApplicationUser>(id);

        user.Phone = resource.Phone;
        user.Office = resource.OfficeId is null ? null : await officeRepository.GetAsync(resource.OfficeId.Value);
        user.Active = resource.Active;

        return await userManager.UpdateAsync(user);
    }

    public void Dispose()
    {
        userManager.Dispose();
        officeRepository.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        userManager.Dispose();
        await officeRepository.DisposeAsync();
    }
}
