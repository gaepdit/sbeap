﻿using FluentValidation;
using Sbeap.AppServices.DtoBase;
using Sbeap.Domain;
using Sbeap.Domain.Entities.Offices;

namespace Sbeap.AppServices.Offices.Validators;

public class OfficeUpdateValidator : AbstractValidator<OfficeUpdateDto>
{
    private readonly IOfficeRepository _repository;

    public OfficeUpdateValidator(IOfficeRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (e, _, token) => await NotDuplicateName(e, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(StandardNamedEntityUpdateDto item, CancellationToken token = default)
    {
        var existing = await _repository.FindByNameAsync(item.Name, token);
        return existing is null || existing.Id == item.Id;
    }
}
