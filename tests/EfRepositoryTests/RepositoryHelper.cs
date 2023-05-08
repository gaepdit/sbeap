using GaEpd.AppLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.Offices;
using Sbeap.EfRepository.Contexts;
using Sbeap.EfRepository.Contexts.SeedDevData;
using Sbeap.EfRepository.Repositories;
using Sbeap.TestData;
using Sbeap.TestData.Identity;
using System.Runtime.CompilerServices;
using TestSupport.EfHelpers;

namespace EfRepositoryTests;

/// <summary>
/// <para>
/// This class can be used to create a new database for each unit test.
/// </para>
/// <para>
/// Use the <see cref="CreateRepositoryHelper"/> method to set up a Sqlite database.
/// </para>
/// <para>
/// If SQL Server-specific features need to be tested, then use <see cref="CreateSqlServerRepositoryHelper"/>.
/// </para>
/// </summary>
public sealed class RepositoryHelper : IDisposable
{
    private AppDbContext Context { get; set; } = default!;

    private readonly DbContextOptions<AppDbContext> _options;
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor used by <see cref="CreateRepositoryHelper"/>.
    /// </summary>
    private RepositoryHelper()
    {
        _options = SqliteInMemory.CreateOptions<AppDbContext>();
        _context = new AppDbContext(_options);
        _context.Database.EnsureCreated();
    }

    /// <summary>
    /// Constructor used by <see cref="CreateSqlServerRepositoryHelper"/>.
    /// </summary>
    /// <param name="callingClass">The class of the unit test method requesting the Repository Helper.</param>
    /// <param name="callingMember">The unit test method requesting the Repository Helper.</param>
    private RepositoryHelper(object callingClass, string callingMember)
    {
        _options = callingClass.CreateUniqueMethodOptions<AppDbContext>(callingMember: callingMember,
            builder: opts => opts.UseSqlServer(
                sqlServerOpts =>
                {
                    // This will no longer be necessary after upgrading to .NET 8.
                    sqlServerOpts.UseDateOnlyTimeOnly();
                }));
        _context = new AppDbContext(_options);
        _context.Database.EnsureClean();
    }

    /// <summary>
    /// Creates a new Sqlite database and returns a RepositoryHelper.
    /// </summary>
    /// <example>
    /// <para>
    /// Create an instance of a <c>RepositoryHelper</c> and a <c>Repository</c> like this:
    /// </para>
    /// <code>
    /// using var repositoryHelper = RepositoryHelper.CreateRepositoryHelper();
    /// using var repository = repositoryHelper.GetOfficeRepository();
    /// </code>
    /// </example>
    /// <returns>A <see cref="RepositoryHelper"/> with an empty Sqlite database.</returns>
    public static RepositoryHelper CreateRepositoryHelper() => new();

    /// <summary>
    /// <para>
    /// Creates a SQL Server database and returns a RepositoryHelper. Use of this method requires that
    /// an "appsettings.json" exists in the project root with a connection string named "UnitTestConnection".
    /// </para>
    /// <para>
    /// (The "<c>callingClass</c>" and "<c>callingMember</c>" parameters are used to generate a unique
    /// database for each unit test method.)
    /// </para>
    /// </summary>
    /// <example>
    /// <para>
    /// Create an instance of a <c>RepositoryHelper</c> and a <c>Repository</c> like this:
    /// </para>
    /// <code>
    /// using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
    /// using var repository = repositoryHelper.GetOfficeRepository();
    /// </code>
    /// </example>
    /// <param name="callingClass">
    /// Enter "<c>this</c>". The class of the unit test method requesting the Repository Helper.
    /// </param>
    /// <param name="callingMember">
    /// Do not enter. The unit test method requesting the Repository Helper. This is filled in by the compiler.
    /// </param>
    /// <returns>A <see cref="RepositoryHelper"/> with a clean SQL Server database.</returns>
    public static RepositoryHelper CreateSqlServerRepositoryHelper(
        object callingClass,
        [CallerMemberName] string callingMember = "") =>
        new(callingClass, callingMember);

    /// <summary>
    /// Stops tracking all currently tracked entities.
    /// See https://github.com/JonPSmith/EfCore.TestSupport/wiki/Using-SQLite-in-memory-databases#1-best-approach-one-instance-and-use-changetrackerclear
    /// </summary>
    public void ClearChangeTracker() => Context.ChangeTracker.Clear();

    /// <summary>
    /// Deletes all data from the EF database table for the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity whose data is to be deleted.</typeparam>
    public async Task ClearTableAsync<TEntity>() where TEntity : AuditableEntity
    {
        Context.RemoveRange(Context.Set<TEntity>());
        await Context.SaveChangesAsync();
        ClearChangeTracker();
    }

    private static void ClearAllStaticData()
    {
        ContactData.ClearData();
        CaseworkData.ClearData();
        AgencyData.ClearData();
        ActionItemData.ClearData();
        ActionItemTypeData.ClearData();
        CustomerData.ClearData();
        OfficeData.ClearData();
        UserData.ClearData();
    }

    /// <summary>
    /// Seeds data for the Agency entity and returns an instance of AgencyRepository.
    /// </summary>
    /// <returns>An <see cref="AgencyRepository"/>.</returns>
    public IAgencyRepository GetAgencyRepository()
    {
        ClearAllStaticData();
        DbSeedDataHelpers.SeedAgencyData(_context);
        Context = new AppDbContext(_options);
        return new AgencyRepository(Context);
    }

    /// <summary>
    /// Seeds data for the Casework entity and returns an instance of CaseworkRepository.
    /// </summary>
    /// <returns>An <see cref="CaseworkRepository"/>.</returns>
    public ICaseworkRepository GetCaseworkRepository()
    {
        ClearAllStaticData();
        DbSeedDataHelpers.SeedAllData(_context);
        Context = new AppDbContext(_options);
        return new CaseworkRepository(Context);
    }

    /// <summary>
    /// Seeds data for the Customer entity and returns an instance of CustomerRepository.
    /// </summary>
    /// <returns>An <see cref="CustomerRepository"/>.</returns>
    public ICustomerRepository GetCustomerRepository()
    {
        ClearAllStaticData();
        DbSeedDataHelpers.SeedAllData(_context);
        Context = new AppDbContext(_options);
        return new CustomerRepository(Context);
    }

    /// <summary>
    /// Seeds data for the Office entity and returns an instance of OfficeRepository.
    /// </summary>
    /// <returns>An <see cref="OfficeRepository"/>.</returns>
    public IOfficeRepository GetOfficeRepository()
    {
        ClearAllStaticData();
        DbSeedDataHelpers.SeedOfficeData(_context);
        Context = new AppDbContext(_options);
        return new OfficeRepository(Context);
    }

    public void Dispose()
    {
        _context.Dispose();
        Context.Dispose();
    }
}
