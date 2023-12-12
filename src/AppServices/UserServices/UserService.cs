using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.UserServices;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        return principal is null ? null : await _userManager.GetUserAsync(principal);
    }

    public Task<ApplicationUser?> FindUserAsync(string id) =>
        _userManager.FindByIdAsync(id);
}
