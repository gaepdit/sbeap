using Microsoft.AspNetCore.Identity;
using Sbeap.Domain.Identity;
using Sbeap.EfRepository.Contexts;
using Sbeap.LocalRepository.Identity;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.AppConfiguration;

public static class IdentityStores
{
    public static IServiceCollection AddIdentityStores(this IServiceCollection services)
    {
        var identityBuilder = services.AddIdentity<ApplicationUser, IdentityRole>();
        services.Configure<IdentityOptions>(options => options.User.RequireUniqueEmail = true);

        if (AppSettings.DevSettings.UseDevSettings && !AppSettings.DevSettings.BuildDatabase)
        {
            // Add local UserStore and RoleStore.
            services.AddSingleton<IUserStore<ApplicationUser>, LocalUserStore>();
            services.AddSingleton<IRoleStore<IdentityRole>, LocalRoleStore>();
        }
        else
        {
            // Add EF identity stores.
            identityBuilder.AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
        }

        return services;
    }
}
