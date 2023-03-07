using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Identity;
using Sbeap.Domain.Offices;

namespace Sbeap.EfRepository.Contexts;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Add domain entities here.
    public DbSet<Office> Offices => Set<Office>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // See https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#model-configuration-for-auto-including-navigations
        builder.Entity<ApplicationUser>().Navigation(e => e.Office).AutoInclude();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    }
}
