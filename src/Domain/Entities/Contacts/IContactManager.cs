using Sbeap.Domain.Entities.Customers;

namespace Sbeap.Domain.Entities.Contacts;

/// <summary>
/// A manager for managing Contact.
/// </summary>
public interface IContactManager
{
    /// <summary>
    /// Creates a new <see cref="Contact"/>.
    /// </summary>
    /// <param name="customer">The <see cref="Customer"/> the Contact was added to.</param>
    /// <returns>The Contact that was created.</returns>
    Contact Create(Customer customer);
}
