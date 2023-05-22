using Sbeap.AppServices.DtoBase;
using Sbeap.AppServices.Offices;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Staff.Dto;

public record StaffViewDto : IDtoHasNameProperty
{
    public string Id { get; init; } = null!;
    public string GivenName { get; init; } = null!;
    public string FamilyName { get; init; } = null!;

    [Display(Name = "Email cannot be changed")]
    public string? Email { get; init; }

    public string? Phone { get; init; }
    public OfficeViewDto? Office { get; init; }
    public bool Active { get; init; }

    // Display properties
    [JsonIgnore]
    public string Name =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));

    public StaffUpdateDto AsUpdateDto() => new()
    {
        Id = Id,
        Phone = Phone,
        OfficeId = Office?.Id,
        Active = Active,
    };
}
