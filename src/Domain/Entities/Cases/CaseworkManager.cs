using Sbeap.Domain.Entities.Customers;

namespace Sbeap.Domain.Entities.Cases;

/// <inheritdoc />
public class CaseworkManager : ICaseworkManager
{
    public Casework Create(Customer customer, DateOnly caseOpenedDate) =>
        new(Guid.NewGuid(), customer, caseOpenedDate);
}
