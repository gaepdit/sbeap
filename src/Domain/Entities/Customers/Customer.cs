using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.ValueObjects;

namespace Sbeap.Domain.Entities.Customers;

public class Customer : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Customer() { }

    internal Customer(Guid id) : base(id) { }

    // Properties

    public string Name { get; set; } = string.Empty;
    public string? County { get; set; }
    public IncompleteAddress? Location { get; set; }
    public IncompleteAddress? MailingAddress { get; set; }
    public string Description { get; set; } = string.Empty;

    [MaxLength(2000)] // https://stackoverflow.com/q/417142/212978
    public string? WebSite { get; set; } = string.Empty;

    // Properties: Action Items
    public List<Casework> Cases { get; set; } = new();
}
