namespace Sbeap.Domain.Entities.ActionItemTypes;

public class ActionItemTypeManager : NamedEntityManager<ActionItemType, IActionItemTypeRepository>, IActionItemTypeManager
{
    public ActionItemTypeManager(IActionItemTypeRepository repository) : base(repository) { }
}
