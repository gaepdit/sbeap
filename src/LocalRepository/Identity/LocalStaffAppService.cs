using AutoMapper;
using GaEpd.AppLibrary.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Identity;
using Sbeap.Domain.Offices;
using Sbeap.TestData.Identity;

namespace Sbeap.LocalRepository.Identity;

public sealed class LocalStaffAppService : IStaffAppService
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IdentityErrorDescriber _errorDescriber;
    private readonly IOfficeRepository _officeRepository;

    public LocalStaffAppService(
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IdentityErrorDescriber errorDescriber,
        IOfficeRepository officeRepository)
    {
        _userService = userService;
        _userManager = userManager;
        _mapper = mapper;
        _errorDescriber = errorDescriber;
        _officeRepository = officeRepository;
    }

    public async Task<StaffViewDto?> GetCurrentUserAsync()
    {
        var user = await _userService.GetCurrentUserAsync();
        return _mapper.Map<StaffViewDto?>(user);
    }

    public Task<StaffViewDto?> FindAsync(Guid id)
    {
        var user = IdentityData.GetUsers.SingleOrDefault(e => e.Id == id.ToString());
        return Task.FromResult(_mapper.Map<StaffViewDto?>(user));
    }

    public async Task<List<StaffViewDto>> GetListAsync(StaffSearchDto filter)
    {
        var users = string.IsNullOrEmpty(filter.Role)
            ? IdentityData.GetUsers.AsQueryable().ApplyFilter(filter)
            : (await _userManager.GetUsersInRoleAsync(filter.Role)).AsQueryable().ApplyFilter(filter);

        return _mapper.Map<List<StaffViewDto>>(users);
    }

    public async Task<IList<string>> GetRolesAsync(Guid id) =>
        await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(id.ToString()));

    public async Task<IList<AppRole>> GetAppRolesAsync(Guid id) => AppRole.RolesAsAppRoles(await GetRolesAsync(id));

    public async Task<IdentityResult> UpdateRolesAsync(Guid id, Dictionary<string, bool> roles)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return IdentityResult.Failed(_errorDescriber.DefaultError());

        foreach (var (role, value) in roles)
        {
            var result = await UpdateUserRoleAsync(user, role, value);
            if (result != IdentityResult.Success) return result;
        }

        return IdentityResult.Success;

        async Task<IdentityResult> UpdateUserRoleAsync(ApplicationUser u, string r, bool addToRole)
        {
            var isInRole = await _userManager.IsInRoleAsync(u, r);
            if (addToRole == isInRole) return IdentityResult.Success;

            return addToRole switch
            {
                true => await _userManager.AddToRoleAsync(u, r),
                false => await _userManager.RemoveFromRoleAsync(u, r),
            };
        }
    }

    public async Task<IdentityResult> UpdateAsync(StaffUpdateDto resource)
    {
        var user = await _userManager.FindByIdAsync(resource.Id.ToString());
        if (user is null) throw new EntityNotFoundException(typeof(ApplicationUser), resource.Id);

        user.Phone = resource.Phone;
        user.Office = resource.OfficeId == null ? null : await _officeRepository.GetAsync(resource.OfficeId.Value);
        user.Active = resource.Active;

        return IdentityResult.Success;
    }

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
