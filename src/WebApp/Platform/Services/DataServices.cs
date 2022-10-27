using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Offices;
using Sbeap.Infrastructure.Contexts;
using Sbeap.Infrastructure.Repositories;
using Sbeap.LocalRepository.Repositories;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.Services;

public static class DataServices
{
    public static void AddDataServices(this IServiceCollection services,
        ConfigurationManager configuration, bool isLocal)
    {
        // When running locally, you have the option to use in-memory data or build the database using LocalDB.
        if (isLocal && !ApplicationSettings.LocalDevSettings.BuildLocalDb)
        {
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseInMemoryDatabase("TEMP_DB"));

            // Uses static data if no database is built.
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
        }
        else
        {
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("Infrastructure")));

            services.AddScoped<IOfficeRepository, OfficeRepository>();
        }
    }
}
