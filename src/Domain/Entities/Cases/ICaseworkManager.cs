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
}
