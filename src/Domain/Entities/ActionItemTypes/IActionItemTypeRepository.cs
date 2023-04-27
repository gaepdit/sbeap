namespace Sbeap.Domain.Entities.ActionItemTypes;

public interface IActionItemTypeRepository : IRepository<ActionItemType, Guid>
{
    /// <summary>
    /// Returns the <see cref="ActionItemType"/> with the given <paramref name="name"/>.
    /// Returns null if the name does not exist.
    /// </summary>
    /// <param name="name">The name of the Action Item Type.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>An Action Item Type entity.</returns>
    Task<ActionItemType?> FindByNameAsync(string name, CancellationToken token = default);
}
