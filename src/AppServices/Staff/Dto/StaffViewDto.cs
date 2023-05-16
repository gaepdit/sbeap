using Sbeap.AppServices.DtoBase;
using Sbeap.AppServices.Offices;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Staff.Dto;

public record StaffViewDto
(
    string Id,
    string GivenName,
    string FamilyName,
    [Display(Name = "Email cannot be changed")]
    string? Email,
    string? Phone,
    OfficeViewDto? Office,
    bool Active
) : IDtoHasNameProperty
{
    [JsonIgnore]
    public string Name =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));

    [JsonIgnore]
    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));

    public StaffUpdateDto AsUpdateDto() => new(Id, Phone, Office?.Id, Active);
}
