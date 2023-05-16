using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Customers.Dto;

public record ContactCreateDto
(
    string? Honorific,
    [Display(Name = "First name")] string? GivenName,
    [Display(Name = "Last name")] string? FamilyName,
    string? Title,
    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    string? Email,
    string? Notes,
    IncompleteAddress Address,
    [Display(Name = "Phone number")] PhoneNumber PhoneNumber
)
{
    public bool IsEmpty() =>
        string.IsNullOrEmpty(Honorific) &&
        string.IsNullOrEmpty(GivenName) &&
        string.IsNullOrEmpty(FamilyName) &&
        string.IsNullOrEmpty(Title) &&
        string.IsNullOrEmpty(Email) &&
        string.IsNullOrEmpty(Notes) &&
        Address == IncompleteAddress.EmptyAddress &&
        PhoneNumber == PhoneNumber.EmptyPhoneNumber;
}
