using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Customers.Dto;

public record CustomerCreateDto
(
    [Required] string Name,
    string? Description,
    string? County,
    [MaxLength(2000)] string? Website, // https://stackoverflow.com/q/417142/212978
    IncompleteAddress Location,
    IncompleteAddress MailingAddress,
    ContactCreateDto Contact
);
