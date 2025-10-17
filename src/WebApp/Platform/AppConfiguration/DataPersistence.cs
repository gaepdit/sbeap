using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Sbeap.Domain.Identity;
using Sbeap.EfRepository.Contexts;
using Sbeap.EfRepository.Contexts.SeedDevData;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.AppConfiguration;

internal static partial class DataPersistence
{
    public static async Task ConfigureDataPersistenceAsync(this IHostApplicationBuilder builder)
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
            .UseSqlServer(connectionString, opts =>
            {
                opts.EnableRetryOnFailure();
                opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
            .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
            .EnableDetailedErrors(builder.Environment.IsDevelopment())
        );

        // Repositories
        builder.Services.AddEntityFrameworkRepositories();
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
        // When configured, build a SQL Server database; otherwise, use in-memory data.
        if (AppSettings.DevSettings.BuildDatabase)
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
        else
        {
            builder.Services.AddInMemoryRepositories();
        }
    }
}
