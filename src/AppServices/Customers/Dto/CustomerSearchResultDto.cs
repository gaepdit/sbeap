using System.ComponentModel.DataAnnotations;

namespace Sbeap.AppServices.Customers.Dto;

public record CustomerSearchResultDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    [Display(Name = "SIC Code")]
    public string? SicCodeDisplay { get; init; }

    public string? County { get; init; }
    public bool IsDeleted { get; [UsedImplicitly] init; }
}
