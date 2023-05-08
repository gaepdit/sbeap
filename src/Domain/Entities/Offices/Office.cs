using Sbeap.Domain.Identity;

namespace Sbeap.Domain.Entities.Offices;

public class Office : AuditableEntity
{
    // Constants

    public const int MaxNameLength = 50;
    public const int MinNameLength = 2;

    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Office() { }

    internal Office(Guid id, string name) : base(id) => SetName(name);

    // Properties

    [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
    public string Name { get; private set; } = string.Empty;

    public bool Active { get; set; } = true;

    public ICollection<ApplicationUser> StaffMembers { get; set; } = new List<ApplicationUser>();

    // Methods

    internal void ChangeName(string name) => SetName(name);

    private void SetName(string name) =>
        Name = Guard.ValidLength(name.Trim(), MinNameLength, MaxNameLength);
}
