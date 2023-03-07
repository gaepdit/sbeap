using Microsoft.EntityFrameworkCore;
using MyAppRoot.Domain.Offices;
using MyAppRoot.EfRepository.Contexts;
using MyAppRoot.EfRepository.Repositories;
using MyAppRoot.LocalRepository.Repositories;
using MyAppRoot.WebApp.Platform.Settings;

namespace MyAppRoot.WebApp.Platform.Services;

public static class DataStores
{
    public static void AddDataStores(this IServiceCollection services, ConfigurationManager configuration)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (ApplicationSettings.DevSettings.UseInMemoryData)
        {
            // Uses local static data if no database is built.
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
        }
        else
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("TEMP_DB"));
            }
            else
            {
                services.AddDbContext<AppDbContext>(opts =>
                    opts.UseSqlServer(connectionString, x => x.MigrationsAssembly("EfRepository")));
            }

            services.AddScoped<IOfficeRepository, OfficeRepository>();
        }
    }
}
