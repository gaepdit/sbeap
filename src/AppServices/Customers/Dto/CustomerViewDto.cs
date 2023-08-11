using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Customers.Dto;

public record CustomerViewDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? County { get; init; }
    public string? Website { get; init; }

    [Display(Name = "Site Location")]
    public IncompleteAddress Location { get; init; } = default!;

    [Display(Name = "Mailing Address")]
    public IncompleteAddress MailingAddress { get; init; } = default!;

    [UsedImplicitly]
    public List<ContactViewDto> Contacts { get; } = new();

    [UsedImplicitly]
    public List<CaseworkSearchResultDto> Cases { get; } = new();

    // Properties: Deletion

    [Display(Name = "Deleted?")]
    public bool IsDeleted { get; init; }

    [Display(Name = "Deleted By")]
    public StaffViewDto? DeletedBy { get; init; }

    [Display(Name = "Date Deleted")]
    public DateTimeOffset? DeletedAt { get; init; }

    [Display(Name = "Deletion Comments")]
    public string? DeleteComments { get; init; }
}
