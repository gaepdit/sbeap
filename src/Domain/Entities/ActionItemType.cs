namespace Sbeap.Domain.Entities;

public abstract class SbeapStandardNamedEntity : StandardNamedEntity
{
    public const int MinimumNameLength = 2;
    public const int MaximumNameLength = 50;
    public override int MinNameLength => MinimumNameLength;
    public override int MaxNameLength => MaximumNameLength;
    protected SbeapStandardNamedEntity() { }
    protected SbeapStandardNamedEntity(Guid id, string name) : base(id, name) { }
}
