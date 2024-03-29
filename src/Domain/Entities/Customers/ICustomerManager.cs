using Sbeap.Domain.Entities.Contacts;
using Sbeap.Domain.ValueObjects;

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
    /// <param name="createdById">The ID of the user creating the entity.</param>
    /// <returns>The Customer that was created.</returns>
    Customer Create(string name, string? createdById);

    /// <summary>
    /// Creates a new <see cref="Contact"/>.
    /// </summary>
    /// <param name="customer">The <see cref="Customer"/> the Contact is to be added to.</param>
    /// <param name="createdById">The ID of the user creating the entity.</param>
    /// <returns>The Contact that was created.</returns>
    Contact CreateContact(Customer customer, string? createdById);

    /// <summary>
    /// Creates a new <see cref="PhoneNumber"/>.
    /// </summary>
    /// <param name="number">The number.</param>
    /// <param name="type">The <see cref="PhoneType"/> of the number.</param>
    /// <returns>The Phone Number that was created.</returns>
    PhoneNumber CreatePhoneNumber(string number, PhoneType type);
}
