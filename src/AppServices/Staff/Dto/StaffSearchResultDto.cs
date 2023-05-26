using Sbeap.Domain.Extensions;
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
    public string SortableFullName => new[] { FamilyName, GivenName }.ConcatWithSeparator(", ");
}
