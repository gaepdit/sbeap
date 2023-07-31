using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Identity;
using Sbeap.Domain.ValueObjects;

namespace Sbeap.Domain.Entities.Contacts;

public class Contact : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Contact() { }

    internal Contact(Guid id, Customer customer) : base(id)
    {
        Customer = customer;
    }

    // Properties

    public Customer Customer { get; private init; } = default!;
    public ApplicationUser? EnteredBy { get; set; }
    public DateTimeOffset? EnteredOn { get; init; }

    public string? Honorific { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Title { get; set; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    public string? Notes { get; set; }
    public IncompleteAddress Address { get; set; } = default!;

    // Collections

    public List<PhoneNumber> PhoneNumbers { get; } = new();
}
