using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Identity;

namespace Sbeap.Domain.Entities.ActionItems;

public class ActionItem : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private ActionItem() { }

    internal ActionItem(Guid id, Casework casework, ActionItemType actionItemType) : base(id)
    {
        Casework = casework;
        ActionItemType = actionItemType;
    }

    // Properties

    public Casework Casework { get; private init; } = default!;
    public ActionItemType ActionItemType { get; private init; } = default!;

    public DateOnly ActionDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public ApplicationUser EnteredBy { get; set; } = default!;
}
