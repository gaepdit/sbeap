namespace Sbeap.Domain.Entities.ActionItems;

public class ActionItem : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private ActionItem() { }

    internal ActionItem(Guid id) : base(id) { }

    // Properties
}
