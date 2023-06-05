using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.Offices;
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
            services.AddSingleton<IActionItemRepository, LocalActionItemRepository>();
            services.AddSingleton<IActionItemTypeRepository, LocalActionItemTypeRepository>();
            services.AddSingleton<IAgencyRepository, LocalAgencyRepository>();
            services.AddSingleton<ICaseworkRepository, LocalCaseworkRepository>();
            services.AddSingleton<IContactRepository, LocalContactRepository>();
            services.AddSingleton<ICustomerRepository, LocalCustomerRepository>();
            services.AddSingleton<IOfficeRepository, LocalOfficeRepository>();
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("TEMP_DB"));
            }
            else
            {
                services.AddDbContext<AppDbContext>(opts =>
                    opts.UseSqlServer(connectionString, sqlServerOpts =>
                    {
                        // DateOnly and TimeOnly entity properties require the following package: 
                        // ErikEJ.EntityFrameworkCore.SqlServer.DateOnlyTimeOnly
                        // This will no longer be necessary after upgrading to .NET 8.
                        sqlServerOpts.UseDateOnlyTimeOnly();
                        sqlServerOpts.MigrationsAssembly("EfRepository");
                    }));
            }

            services.AddScoped<IActionItemRepository, ActionItemRepository>();
            services.AddScoped<IAgencyRepository, AgencyRepository>();
            services.AddScoped<ICaseworkRepository, CaseworkRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
        }
    }
}
