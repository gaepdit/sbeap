using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Identity;
using Sbeap.EfRepository.Contexts;
using Sbeap.EfRepository.Contexts.SeedDevData;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.Services;

public class MigratorHostedService : IHostedService
{
    // Inject the IServiceProvider so we can create the DbContext scoped service.
    private readonly IServiceProvider _serviceProvider;
    public MigratorHostedService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // If using in-memory data, no further action required.
        if (ApplicationSettings.DevSettings.UseInMemoryData) return;

        // Retrieve scoped services.
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        if (ApplicationSettings.DevSettings.UseEfMigrations)
        {
            // Run any database migrations if used.
            await context.Database.MigrateAsync(cancellationToken);
        }
        else
        {
            // Otherwise, delete and re-create the database.
            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }

        // If running in the development environment, add seed data to database.
        if (env.IsDevelopment()) DbSeedDataHelpers.SeedAllData(context);
    }

    // noop
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
