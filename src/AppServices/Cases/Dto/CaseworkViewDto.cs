﻿using Sbeap.AppServices.Customers.Dto;

namespace Sbeap.AppServices.Cases.Dto;

public class CaseworkViewDto
{
    public Guid Id { get; init; }
    public CustomerSearchResultDto Customer { get; init; } = default!;
    public string Description { get; init; } = string.Empty;
    public DateOnly CaseOpenedDate { get; init; }
    public DateOnly? CaseClosedDate { get; init; }
    public bool IsClosed { get; init; }
    public string? CaseClosureNotes { get; init; }
    public string? ReferralAgencyName { get; init; }
    public DateOnly? ReferralDate { get; init; }
    public string? ReferralNotes { get; init; }
    public ICollection<ActionItemViewDto> ActionItems { get; init; } = new List<ActionItemViewDto>();
    public bool IsDeleted { get; init; }
}
