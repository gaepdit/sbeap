namespace Sbeap.Domain.Entities.Cases;

public interface ICaseworkRepository : IRepository<Casework, Guid>
{
    /// <summary>
    /// Returns the <see cref="Casework"/> with the given <paramref name="id"/> and includes
    /// all additional properties (Action Items).
    /// Returns null if no Casework exists with the given Id.
    /// </summary>
    /// <param name="id">The Id of the Casework.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A Casework entity.</returns>
    Task<Casework?> FindIncludeAllAsync(Guid id, CancellationToken token = default);
}
