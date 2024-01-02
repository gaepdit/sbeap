using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.UserServices;

public class UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    : IUserService
{
    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var principal = httpContextAccessor.HttpContext?.User;
        return principal is null ? null : await userManager.GetUserAsync(principal);
    }

    public Task<ApplicationUser?> FindUserAsync(string id) =>
        userManager.FindByIdAsync(id);
}
