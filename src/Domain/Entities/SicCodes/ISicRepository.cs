namespace Sbeap.Domain.Entities.SicCodes;

public interface ISicRepository : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Returns the <see cref="SicCode"/> with the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The Id of the SicCode.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException">Thrown if no entity exists with the given Id.</exception>
    /// <returns>An SicCode.</returns>
    Task<SicCode> GetAsync(string id, CancellationToken token = default);

    /// <summary>
    /// Returns a read-only collection of all <see cref="SicCode"/> values.
    /// Returns an empty collection if none exist.
    /// </summary>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A read-only collection of SicCode entities.</returns>
    Task<IReadOnlyCollection<SicCode>> GetListAsync(CancellationToken token = default);
}
