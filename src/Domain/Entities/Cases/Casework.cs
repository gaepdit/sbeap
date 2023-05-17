using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Customers;

namespace Sbeap.Domain.Entities.Cases;

public class Casework : AuditableSoftDeleteEntity
{
    // Constructors

    [UsedImplicitly] // Used by ORM.
    private Casework() { }

    internal Casework(Guid id, Customer customer, DateOnly caseOpenedDate) : base(id)
    {
        Customer = customer;
        CaseOpenedDate = caseOpenedDate;
    }

    // Properties

    public Customer Customer { get; private init; } = default!;

    public string Description { get; set; } = string.Empty;
    public DateOnly CaseOpenedDate { get; set; }
    public DateOnly? CaseClosedDate { get; set; }
    public bool IsClosed => CaseClosedDate is not null;
    public string? CaseClosureNotes { get; set; }
    public Agency? ReferralAgency { get; set; }
    public DateOnly? ReferralDate { get; set; }
    public string? ReferralNotes { get; set; }

    // Collections

    public List<ActionItem> ActionItems { get; set; } = new();
}
