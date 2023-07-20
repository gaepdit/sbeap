using Sbeap.Domain.Exceptions;

namespace Sbeap.Domain.Entities.ActionItemTypes;

/// <summary>
/// A manager for managing Action Item Types.
/// </summary>
public interface IActionItemTypeManager
{
    /// <summary>
    /// Creates a new <see cref="ActionItemType"/>.
    /// </summary>
    /// <param name="name">The name of the Action Item Type to create.</param>
    /// <param name="createdById">The ID of the user creating the entity.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="NameAlreadyExistsException">Thrown if an Action Item Type already exists with the given name.</exception>
    /// <returns>The Action Item Type that was created.</returns>
    Task<ActionItemType> CreateAsync(string name, string? createdById, CancellationToken token = default);

    /// <summary>
    /// Changes the name of an <see cref="ActionItemType"/>.
    /// </summary>
    /// <param name="actionItemType">The Action Item Type to modify.</param>
    /// <param name="name">The new name for the Action Item Type.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="NameAlreadyExistsException">Thrown if an Action Item Type already exists with the given name.</exception>
    Task ChangeNameAsync(ActionItemType actionItemType, string name, CancellationToken token = default);
}
