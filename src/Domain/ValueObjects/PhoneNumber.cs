using GaEpd.AppLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Sbeap.Domain.ValueObjects;

[Owned]
public record PhoneNumber : ValueObject
{
    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone number")]
    public string? Number { get; [UsedImplicitly] init; }

    [Display(Name = "Phone type")]
    public PhoneType? Type { get; [UsedImplicitly] init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number ?? string.Empty;
        yield return Type ?? PhoneType.Unknown;
    }

    public static PhoneNumber EmptyPhoneNumber => new();
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PhoneType
{
    Work = 0,
    [Display(Name = "Work Cell")] WorkCell = 1,
    Fax = 2,
    Personal = 3,
    [Display(Name = "Personal Cell")] PersonalCell = 4,
    Other = 5,
    Unknown = 6,
}
