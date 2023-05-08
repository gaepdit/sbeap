using Sbeap.Domain.Entities.Contacts;

namespace Sbeap.Domain.Entities.Customers;

/// <summary>
/// A manager for managing Customers and Contacts.
/// </summary>
public interface ICustomerManager
{
    /// <summary>
    /// Creates a new <see cref="Customer"/>.
    /// </summary>
    /// <param name="name">The Name of the Customer to create.</param>
    /// <returns>The Customer that was created.</returns>
    Customer Create(string name);

    /// <summary>
    /// Creates a new <see cref="Contact"/>.
    /// </summary>
    /// <param name="customer">The <see cref="Customer"/> the Contact is to be added to.</param>
    /// <returns>The Contact that was created.</returns>
    Contact CreateContact(Customer customer);
}
