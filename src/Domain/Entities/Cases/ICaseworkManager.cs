using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Customers;

namespace Sbeap.Domain.Entities.Cases;

/// <summary>
/// A manager for managing Casework.
/// </summary>
public interface ICaseworkManager
{
    /// <summary>
    /// Creates a new <see cref="Casework"/>.
    /// </summary>
    /// <param name="customer">The <see cref="Customer"/> the Casework was opened for.</param>
    /// <param name="caseOpenedDate">The date the Casework was opened.</param>
    /// <returns>The Casework that was created.</returns>
    Casework Create(Customer customer, DateOnly caseOpenedDate);

    /// <summary>
    /// Creates a new <see cref="ActionItem"/>.
    /// </summary>
    /// <param name="casework">The <see cref="Casework"/> this Action Item belongs to.</param>
    /// <param name="actionItemType">The <see cref="ActionItemType"/> of this Action Item.</param>
    /// <returns>The Action Item that was created.</returns>
    ActionItem CreateActionItem(Casework casework, ActionItemType actionItemType);
}
