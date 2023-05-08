using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Customers;

namespace Sbeap.Domain.Entities.Cases;

/// <inheritdoc />
public class CaseworkManager : ICaseworkManager
{
    public Casework Create(Customer customer, DateOnly caseOpenedDate) =>
        new(Guid.NewGuid(), customer, caseOpenedDate);

    public ActionItem CreateActionItem(Casework casework, ActionItemType actionItemType) =>
        new(Guid.NewGuid(), casework, actionItemType);
}
