using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;

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

    public Customer Customer { get; init; } = default!;
    public DateTimeOffset EnteredDate { get; init; } = DateTimeOffset.Now;

    public DateOnly CaseOpenedDate { get; set; }
    public DateOnly? CaseClosedDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public Office? InteragencyReferral { get; set; } = default!;
    public string ReferralInformation { get; set; } = string.Empty;
    public DateOnly? ReferralDate { get; set; }
    public ApplicationUser EnteredBy { get; set; } = default!;

    // Properties: Action Items
    public List<ActionItem> ActionItems { get; set; } = new();
}
