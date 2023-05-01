namespace Sbeap.Domain.Entities.Agencies;

public interface IAgencyRepository : IRepository<Agency, Guid>
{
    /// <summary>
    /// Returns the <see cref="Agency"/> with the given <paramref name="name"/>.
    /// Returns null if the name does not exist.
    /// </summary>
    /// <param name="name">The Name of the Agency.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An Agency entity.</returns>
    Task<Agency?> FindByNameAsync(string name, CancellationToken token = default);
}
