using Sbeap.Domain.Exceptions;

namespace Sbeap.Domain.Entities.Agencies;

/// <summary>
/// A manager for managing Agencies.
/// </summary>
public interface IAgencyManager
{
    /// <summary>
    /// Creates a new <see cref="Agency"/>.
    /// </summary>
    /// <param name="name">The name of the Agency to create.</param>
    /// <param name="createdById"></param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="NameAlreadyExistsException">Thrown if an Agency already exists with the given name.</exception>
    /// <returns>The Agency that was created.</returns>
    Task<Agency> CreateAsync(string name, string? createdById, CancellationToken token = default);

    /// <summary>
    /// Changes the name of an <see cref="Agency"/>.
    /// </summary>
    /// <param name="agency">The Agency to modify.</param>
    /// <param name="name">The new name for the Agency.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="NameAlreadyExistsException">Thrown if an Agency already exists with the given name.</exception>
    Task ChangeNameAsync(Agency agency, string name, CancellationToken token = default);
}
