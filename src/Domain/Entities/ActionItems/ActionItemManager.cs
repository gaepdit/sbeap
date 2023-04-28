using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Cases;

namespace Sbeap.Domain.Entities.ActionItems;

/// <inheritdoc />
public class ActionItemManager : IActionItemManager
{
    public ActionItem Create(Casework casework, ActionItemType actionItemType) =>
        new(Guid.NewGuid(), casework, actionItemType);
}
