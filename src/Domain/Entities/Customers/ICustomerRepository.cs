namespace Sbeap.Domain.Entities.Customers;

public interface ICustomerRepository : IRepository<Customer, Guid>
{
    /// <summary>
    /// Returns the <see cref="Customer"/> with the given <paramref name="id"/> and includes
    /// all additional properties (Contacts and Cases).
    /// Returns null if no Customer exists with the given Id.
    /// </summary>
    /// <param name="id">The Id of the Customer.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns>A Customer entity.</returns>
    Task<Customer?> FindIncludeAllAsync(Guid id, CancellationToken token = default);
}
