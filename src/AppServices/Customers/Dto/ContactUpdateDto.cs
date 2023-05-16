using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Customers.Dto;

public record ContactUpdateDto
(
    Guid Id,
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
    IncompleteAddress Address
);
