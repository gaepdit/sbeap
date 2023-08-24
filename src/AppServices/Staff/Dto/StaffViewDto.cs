using GaEpd.AppLibrary.Extensions;
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

    [Display(Name = "Email (cannot be changed)")]
    public string? Email { get; init; }

    public string? Phone { get; init; }
    public OfficeViewDto? Office { get; init; }
    public bool Active { get; init; }

    // Display properties
    [JsonIgnore]
    public string Name => new[] { GivenName, FamilyName }.ConcatWithSeparator();

    [JsonIgnore]
    public string SortableFullName => new[] { FamilyName, GivenName }.ConcatWithSeparator(", ");

    public StaffUpdateDto AsUpdateDto() => new()
    {
        Id = Id,
        Phone = Phone,
        OfficeId = Office?.Id,
        Active = Active,
    };
}
