namespace Sbeap.Domain.Entities.ActionItemTypes;

public class ActionItemTypeManager(IActionItemTypeRepository repository) 
    : NamedEntityManager<ActionItemType, IActionItemTypeRepository>(repository), IActionItemTypeManager;
