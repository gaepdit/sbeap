using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.UserServices;

public class UserService : IUserService
{
    private static readonly TimeSpan UserListExpiration = TimeSpan.FromHours(1);
    private static readonly TimeSpan CurrentUserExpiration = TimeSpan.FromHours(1);
    private const string CurrentUserCacheKey = nameof(CurrentUserCacheKey);

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMemoryCache _cache;

    public UserService(
        UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _cache = cache;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var user = _cache.Get<ApplicationUser>(CurrentUserCacheKey);
        if (user is not null) return user;

        var principal = _httpContextAccessor.HttpContext?.User;
        user = principal is null ? null : await _userManager.GetUserAsync(principal);
        if (user is not null) _cache.Set(CurrentUserCacheKey, user, CurrentUserExpiration);
        return user;
    }

    public async Task<ApplicationUser?> FindUserAsync(string id)
    {
        var user = _cache.Get<ApplicationUser>(id);
        if (user is not null) return user;

        user = await _userManager.FindByIdAsync(id);
        if (user is not null) _cache.Set(id, user, UserListExpiration);
        return null;
    }
}
