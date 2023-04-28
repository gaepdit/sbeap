namespace Sbeap.Domain.Entities.Cases;

public class Casework : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Casework() { }

    internal Casework(Guid id) : base(id) { }

    // Properties
}
