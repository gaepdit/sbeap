using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Customers.Dto;

public class ContactViewDto
{
    public Guid Id { get; init; }
    public string Honorific { get; init; } = string.Empty;
    public string GivenName { get; init; } = string.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; init; } = string.Empty;

    public string Notes { get; init; } = string.Empty;
    public IncompleteAddress Address { get; init; } = default!;

    [Display(Name = "Phone Numbers")]
    public ICollection<PhoneNumber> PhoneNumbers { get; } = new List<PhoneNumber>();

    public bool IsDeleted { get; init; }

    public DateTimeOffset? EnteredOn { get; init; }

    // Read-only properties

    [JsonIgnore]
    public string Name =>
        string.Join(" ", new[] { Honorific, GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));
}
