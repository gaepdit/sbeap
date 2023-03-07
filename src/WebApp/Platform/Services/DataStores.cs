using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Offices;
using Sbeap.EfRepository.Contexts;
using Sbeap.EfRepository.Repositories;
using Sbeap.LocalRepository.Repositories;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.Services;

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
