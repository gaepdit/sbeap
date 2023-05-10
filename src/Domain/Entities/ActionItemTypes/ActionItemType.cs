using Sbeap.Domain.Entities.EntityBase;

namespace Sbeap.Domain.Entities.ActionItemTypes;

public class ActionItemType : SimpleNamedEntity
{
    public ActionItemType(Guid id, string name) : base(id, name) { }
}
