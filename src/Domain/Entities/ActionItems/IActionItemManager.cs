using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Cases;

namespace Sbeap.Domain.Entities.ActionItems;

/// <summary>
/// A manager for managing Action Items.
/// </summary>
public interface IActionItemManager
{
    /// <summary>
    /// Creates a new <see cref="ActionItem"/>.
    /// </summary>
    /// <param name="casework">The <see cref="Casework"/> this Action Item belongs to.</param>
    /// <param name="actionItemType">The <see cref="ActionItemType"/> of this Action Item.</param>
    /// <returns>The Action Item that was created.</returns>
    ActionItem Create(Casework casework, ActionItemType actionItemType);
}
