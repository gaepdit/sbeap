using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Customers.Dto;

public record ContactViewDto
(
    Guid Id,
    string Honorific,
    string GivenName,
    string FamilyName,
    string Title,
    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    string Email,
    string Notes,
    IncompleteAddress Address,
    bool IsDeleted,
    DateTimeOffset? EnteredOn
)
{
    [Display(Name = "Phone Numbers")]
    public ICollection<PhoneNumber> PhoneNumbers { get; } = new List<PhoneNumber>();

    // Read-only properties
    [JsonIgnore]
    public string Name =>
        string.Join(" ", new[] { Honorific, GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));
}
