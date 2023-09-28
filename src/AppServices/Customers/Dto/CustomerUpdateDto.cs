using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Customers.Dto;

public record CustomerUpdateDto
{
    // Authorization handler assist properties
    public bool IsDeleted { get; [UsedImplicitly] init; }

    // Entity update properties

    [Required]
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
    public string? County { get; init; }

    [MaxLength(2000)] // https://stackoverflow.com/q/417142/212978
    public string? Website { get; init; }

    public IncompleteAddress Location { get; init; } = default!;
    public IncompleteAddress MailingAddress { get; init; } = default!;
}
