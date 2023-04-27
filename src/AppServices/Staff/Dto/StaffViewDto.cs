using Sbeap.AppServices.Offices;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Staff.Dto;

public class StaffViewDto
{
    public string Id { get; init; } = string.Empty;
    public string GivenName { get; init; } = string.Empty;
    public string FamilyName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public OfficeViewDto? Office { get; init; }

    [UIHint("BoolActive")]
    public bool Active { get; init; }

    [JsonIgnore]
    public string DisplayName =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));

    public StaffUpdateDto AsUpdateDto() =>
        new() { Id = Id, Phone = Phone, OfficeId = Office?.Id, Active = Active };
}
