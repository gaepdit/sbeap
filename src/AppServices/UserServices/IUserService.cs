using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.UserServices;

public interface IUserService
{
    public Task<ApplicationUser?> GetCurrentUserAsync();
    public Task<ApplicationUser> GetUserAsync(string id);
    public Task<ApplicationUser?> FindUserAsync(string id);
}
