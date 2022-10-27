using Microsoft.AspNetCore.Identity;
using Sbeap.LocalRepository.ServiceCollectionExtensions;
using Sbeap.AppServices.Staff;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Identity;
using Sbeap.Infrastructure.Contexts;
using Sbeap.Infrastructure.Identity;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.Services;

public static class IdentityStores
{
    public static void AddIdentityStores(this IServiceCollection services, bool isLocal)
    {
        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();

        // When running locally, you have the option to use in-memory data or build the database using LocalDB.
        if (isLocal && !ApplicationSettings.LocalDevSettings.BuildLocalDb)
        {
            // Adds local UserStore, RoleSore, and StaffAppService
            services.AddLocalIdentity();
        }
        else
        {
            // Add EF identity stores
            identityBuilder.AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            // Add Staff App Services
            services.AddTransient<IStaffAppService, StaffAppService>();
        }

        services.AddScoped<IUserService, UserService>();
    }
}
