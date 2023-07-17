﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sbeap.EfRepository.Contexts;

/// <summary>
/// Facilitates some EF Core Tools commands. See "Design-time DbContext Creation":
/// https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=sbeap-app;",
            x => x.UseDateOnlyTimeOnly());
        return new AppDbContext(optionsBuilder.Options);
    }
}
