using GaEpd.AppLibrary.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sbeap.Domain;
using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Entities.SicCodes;
using Sbeap.Domain.Identity;

namespace Sbeap.EfRepository.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // Add domain entities here.
    public DbSet<ActionItem> ActionItems => Set<ActionItem>();
    public DbSet<ActionItemType> ActionItemTypes => Set<ActionItemType>();
    public DbSet<Agency> Agencies => Set<Agency>();
    public DbSet<Casework> Cases => Set<Casework>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<SicCode> SicCodes => Set<SicCode>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Some properties should always be included.
        // See https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager#model-configuration-for-auto-including-navigations
        builder.Entity<ActionItem>().Navigation(item => item.ActionItemType).AutoInclude();
        builder.Entity<ActionItem>().Navigation(item => item.Casework).AutoInclude();
        builder.Entity<ApplicationUser>().Navigation(user => user.Office).AutoInclude();
        builder.Entity<Casework>().Navigation(casework => casework.Customer).AutoInclude();
        builder.Entity<Contact>().Navigation(contact => contact.Customer).AutoInclude();
        builder.Entity<Customer>().Navigation(customer => customer.SicCode).AutoInclude();

        // Let's save enums in the database as strings.
        // See https://stackoverflow.com/a/55260541/212978
        builder.Entity<Contact>().OwnsMany(contact => contact.PhoneNumbers,
            navigationBuilder => navigationBuilder.Property(phoneNumber => phoneNumber.Type).HasConversion<string>());

        // Set max length of Name property for StandardNamedEntity
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(type => typeof(StandardNamedEntity).IsAssignableFrom(type.ClrType))
                     .Select(type => type.ClrType))
        {
            builder.Entity(entityType).Property<string>(nameof(StandardNamedEntity.Name))
                .HasMaxLength(AppConstants.MaximumNameLength);
        }


        // ## The following configurations are Sqlite only. ##
        if (Database.ProviderName != "Microsoft.EntityFrameworkCore.Sqlite") return;

        // Sqlite and EF Core are in conflict on how to handle collections of owned types.
        // See: https://stackoverflow.com/a/69826156/212978
        // and: https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities#collections-of-owned-types
        builder.Entity<Contact>().OwnsMany(contact => contact.PhoneNumbers,
            navigationBuilder => navigationBuilder.HasKey("Id"));

        // "Handling DateTimeOffset in SQLite with Entity Framework Core"
        // https://blog.dangl.me/archive/handling-datetimeoffset-in-sqlite-with-entity-framework-core/
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var dateTimeOffsetProperties = entityType.ClrType.GetProperties()
                .Where(info =>
                    info.PropertyType == typeof(DateTimeOffset) || info.PropertyType == typeof(DateTimeOffset?));
            foreach (var property in dateTimeOffsetProperties)
                builder.Entity(entityType.Name).Property(property.Name)
                    .HasConversion(new DateTimeOffsetToBinaryConverter());
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    }
}
