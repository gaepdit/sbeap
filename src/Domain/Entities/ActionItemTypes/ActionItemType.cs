namespace Sbeap.Domain.Entities.ActionItemTypes;

public class ActionItemType : AuditableEntity
{
    // Constants

    public const int MaxNameLength = 450;
    public const int MinNameLength = 2;

    // Constructors

    [UsedImplicitly] // Used by ORM.
    private ActionItemType() { }

    internal ActionItemType(Guid id, string name) : base(id) => SetName(name);

    // Properties

    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; private set; } = string.Empty;

    public bool Active { get; set; } = true;

    // Methods

    internal void ChangeName(string name) => SetName(name);

    private void SetName(string name) =>
        Name = Guard.ValidLength(name.Trim(), MinNameLength, MaxNameLength);
}
