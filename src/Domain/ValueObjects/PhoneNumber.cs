using GaEpd.AppLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Sbeap.Domain.ValueObjects;

[Owned]
public record PhoneNumber : ValueObject
{
    public int Id { get; init; }

    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone number")]
    public string? Number { get; [UsedImplicitly] init; }

    [Display(Name = "Phone type")]
    [Column(TypeName = "nvarchar(25)")]
    public PhoneType? Type { get; [UsedImplicitly] init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number ?? string.Empty;
        // ReSharper disable once HeapView.BoxingAllocation
        yield return Type ?? PhoneType.Unknown;
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PhoneType
{
    [UsedImplicitly] Work = 0,

    [UsedImplicitly, Display(Name = "Work Cell")]
    WorkCell = 1,

    [UsedImplicitly] Fax = 2,

    [UsedImplicitly] Personal = 3,

    [UsedImplicitly, Display(Name = "Personal Cell")]
    PersonalCell = 4,

    [UsedImplicitly] Other = 5,

    [UsedImplicitly] Unknown = 6,
}
