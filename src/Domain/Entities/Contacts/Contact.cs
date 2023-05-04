using Sbeap.Domain.Entities.Customers;
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

    public string Honorific { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;
    public IncompleteAddress Address { get; set; } = default!;

    // Collections

    public ICollection<PhoneNumber> PhoneNumbers { get; } = new List<PhoneNumber>();
}
