using System.Text.Json.Serialization;

namespace Sbeap.AppServices.Staff.Dto;

public record StaffSearchResultDto
(
    string Id,
    string GivenName,
    string FamilyName,
    string Email,
    string? OfficeName,
    bool Active
)
{
    [JsonIgnore]
    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));
}
