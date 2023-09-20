namespace Sbeap.Domain.Entities;

public abstract class SbeapStandardNamedEntity : StandardNamedEntity
{
    public override int MinNameLength => AppConstants.MinimumNameLength;
    public override int MaxNameLength => AppConstants.MaximumNameLength;
    protected SbeapStandardNamedEntity() { }
    protected SbeapStandardNamedEntity(Guid id, string name) : base(id, name) { }
}
