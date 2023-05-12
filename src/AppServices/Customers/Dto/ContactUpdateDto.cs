using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Customers.Dto;

public class ContactUpdateDto
{
    public Guid Id { get; init; }

    public string? Honorific { get; init; } = string.Empty;

    [Display(Name = "First name")]
    public string? GivenName { get; init; } = string.Empty;

    [Display(Name = "Last name")]
    public string? FamilyName { get; init; } = string.Empty;

    public string? Title { get; init; } = string.Empty;

    [EmailAddress]
    [StringLength(150)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string? Email { get; init; } = string.Empty;

    public string? Notes { get; init; } = string.Empty;
    public IncompleteAddress Address { get; init; } = default!;
}
