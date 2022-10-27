using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.Staff;
using Sbeap.Domain.Identity;
using Sbeap.LocalRepository.Identity;

namespace Sbeap.LocalRepository.ServiceCollectionExtensions;

public static class IdentityServices
{
    public static void AddLocalIdentity(this IServiceCollection services)
    {
        services.AddSingleton<IUserStore<ApplicationUser>, LocalUserStore>();
        services.AddSingleton<IRoleStore<IdentityRole>, LocalRoleStore>();
        services.AddTransient<IStaffAppService, LocalStaffAppService>();
    }
}
