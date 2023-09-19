using Sbeap.Domain.Identity;

namespace Sbeap.Domain.Entities.Offices;

public class Office : SbeapStandardNamedEntity
{
    public Office() { }
    public Office(Guid id, string name) : base(id, name) { }

    [UsedImplicitly]
    public ICollection<ApplicationUser> StaffMembers { get; set; } = new List<ApplicationUser>();
}
