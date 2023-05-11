using Sbeap.AppServices.DtoBase;
using Sbeap.AppServices.Offices;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Staff.Dto;

public class StaffViewDto : IDtoHasNameProperty
{
    public string Id { get; init; } = string.Empty;
    public string GivenName { get; init; } = string.Empty;
    public string FamilyName { get; init; } = string.Empty;

    [Display(Name = "Email cannot be changed")]
    public string? Email { get; init; }

    public string? Phone { get; init; }
    public OfficeViewDto? Office { get; init; }

    [UIHint("BoolActive")]
    public bool Active { get; init; }

    [JsonIgnore]
    public string Name =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));

    public StaffUpdateDto AsUpdateDto() =>
        new() { Id = Id, Phone = Phone, OfficeId = Office?.Id, Active = Active };
}
