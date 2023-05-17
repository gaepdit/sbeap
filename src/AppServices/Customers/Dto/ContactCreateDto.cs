using GaEpd.AppLibrary.Domain.ValueObjects;
using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Customers.Dto;

public record ContactCreateDto : ValueObject
{
    public string? Honorific { get; init; }

    [Display(Name = "First name")]
    public string? GivenName { get; init; }

    [Display(Name = "Last name")]
    public string? FamilyName { get; init; }

    public string? Title { get; init; }

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string? Email { get; init; }

    public string? Notes { get; init; }
    public IncompleteAddress Address { get; init; } = default!;

    [Display(Name = "Phone number")]
    public PhoneNumber PhoneNumber { get; init; } = default!;

    public static ContactCreateDto EmptyContact => new()
    {
        Address = IncompleteAddress.EmptyAddress,
        PhoneNumber = PhoneNumber.EmptyPhoneNumber,
    };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Honorific ?? string.Empty;
        yield return GivenName ?? string.Empty;
        yield return FamilyName ?? string.Empty;
        yield return Title ?? string.Empty;
        yield return Email ?? string.Empty;
        yield return Notes ?? string.Empty;
        yield return Address;
        yield return PhoneNumber;
    }
}
