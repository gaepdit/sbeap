namespace Sbeap.Domain.Entities.Contacts;

public class Contact : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Contact() { }

    internal Contact(Guid id) : base(id) { }

    // Properties
}
