using Sbeap.AppServices.Staff.Dto;
using Sbeap.Domain.Extensions;
using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Customers.Dto;

public record ContactViewDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; set; }

    [UsedImplicitly]
    public string Honorific { get; init; } = string.Empty;

    [UsedImplicitly]
    public string GivenName { get; init; } = string.Empty;

    [UsedImplicitly]
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

    public StaffViewDto? EnteredBy { get; init; }

    [Display(Name = "Entered On")]
    public DateTimeOffset? EnteredOn { get; init; }

    // Read-only properties
    [JsonIgnore]
    public string Name => new[] { Honorific, GivenName, FamilyName }.ConcatWithSeparator();
}
