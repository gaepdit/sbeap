using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Sbeap.AppServices.Customers.Dto;

public record PhoneNumberCreate
{
    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone number")]
    public string? Number { get; [UsedImplicitly] init; }

    [Display(Name = "Phone type")]
    public PhoneType? Type { get; [UsedImplicitly] init; }

    [MemberNotNullWhen(false, nameof(Number))]
    [MemberNotNullWhen(false, nameof(Type))]
    public bool IsIncomplete => string.IsNullOrEmpty(Number) || Type == null;
}
