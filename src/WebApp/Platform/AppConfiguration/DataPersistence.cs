using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Entities.SicCodes;
using Sbeap.Domain.Identity;
using Sbeap.EfRepository.Contexts;
using Sbeap.EfRepository.Contexts.SeedDevData;
using Sbeap.EfRepository.Repositories;
using Sbeap.LocalRepository.Repositories;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.AppConfiguration;

public static class DataPersistence
{
    public static async Task ConfigureDataPersistence(this IHostApplicationBuilder builder)
    {
        if (AppSettings.DevSettings.UseDevSettings)
        {
            await builder.ConfigureDevDataPersistence();
            return;
        }

        builder.ConfigureDatabaseServices();

        await using var migrationContext = new AppDbContext(GetMigrationDbOpts(builder.Configuration).Options);
        await migrationContext.Database.MigrateAsync();
        await migrationContext.CreateMissingRolesAsync(builder.Services);
    }

    private static void ConfigureDatabaseServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("No connection string found.");

        // Entity Framework context
        builder.Services.AddDbContext<AppDbContext>(db => db
            .UseSqlServer(connectionString, sqlServerOpts => sqlServerOpts.EnableRetryOnFailure())
            .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning)));

        // Repositories
        builder.Services
            .AddScoped<IActionItemRepository, ActionItemRepository>()
            .AddScoped<IActionItemTypeRepository, ActionItemTypeRepository>()
            .AddScoped<IAgencyRepository, AgencyRepository>()
            .AddScoped<ICaseworkRepository, CaseworkRepository>()
            .AddScoped<IContactRepository, ContactRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IOfficeRepository, OfficeRepository>()
            .AddScoped<ISicRepository, SicRepository>();
    }

    private static DbContextOptionsBuilder<AppDbContext> GetMigrationDbOpts(IConfiguration configuration)
    {
        var migConnString = configuration.GetConnectionString("MigrationConnection");
        if (string.IsNullOrEmpty(migConnString))
            throw new InvalidOperationException("No migration connection string found.");

        return new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(migConnString, sqlServerOpts => sqlServerOpts.MigrationsAssembly(nameof(EfRepository)));
    }

    private static async Task CreateMissingRolesAsync(this AppDbContext migrationContext, IServiceCollection services)
    {
        // Initialize any new roles.
        var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in AppRole.AllRoles.Keys)
            if (!await migrationContext.Roles.AnyAsync(idRole => idRole.Name == role))
                await roleManager.CreateAsync(new IdentityRole(role));
    }

    private static async Task ConfigureDevDataPersistence(this IHostApplicationBuilder builder)
    {
        // When configured, use in-memory data; otherwise use a SQL Server database.
        if (AppSettings.DevSettings.UseInMemoryData)
        {
            builder.Services
                .AddSingleton<IActionItemRepository, LocalActionItemRepository>()
                .AddSingleton<IActionItemTypeRepository, LocalActionItemTypeRepository>()
                .AddSingleton<IAgencyRepository, LocalAgencyRepository>()
                .AddSingleton<ICaseworkRepository, LocalCaseworkRepository>()
                .AddSingleton<IContactRepository, LocalContactRepository>()
                .AddSingleton<ICustomerRepository, LocalCustomerRepository>()
                .AddSingleton<IOfficeRepository, LocalOfficeRepository>()
                .AddSingleton<ISicRepository, LocalSicRepository>();
        }
        else
        {
            builder.ConfigureDatabaseServices();

            await using var migrationContext = new AppDbContext(GetMigrationDbOpts(builder.Configuration).Options);
            await migrationContext.Database.EnsureDeletedAsync();

            if (AppSettings.DevSettings.UseEfMigrations)
                await migrationContext.Database.MigrateAsync();
            else
                await migrationContext.Database.EnsureCreatedAsync();

            DbSeedDataHelpers.SeedAllData(migrationContext);
        }
    }
}
