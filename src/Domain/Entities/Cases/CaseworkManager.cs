using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Customers;

namespace Sbeap.Domain.Entities.Cases;

public class CaseworkManager : ICaseworkManager
{
    public Casework Create(Customer customer, DateOnly caseOpenedDate, string? createdById)
    {
        var item = new Casework(Guid.NewGuid(), customer, caseOpenedDate);
        item.SetCreator(createdById);
        return item;
    }

    public ActionItem CreateActionItem(Casework casework, ActionItemType actionItemType, string? createdById)
    {
        var item = new ActionItem(Guid.NewGuid(), casework, actionItemType);
        item.SetCreator(createdById);
        return item;
    }
}
