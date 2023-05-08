namespace Sbeap.Domain.Entities.Customers;

/// <summary>
/// A manager for managing Customer.
/// </summary>
public interface ICustomerManager
{
    /// <summary>
    /// Creates a new <see cref="Customer"/>.
    /// </summary>
    /// <returns>The Customer that was created.</returns>
    Customer Create();
}
