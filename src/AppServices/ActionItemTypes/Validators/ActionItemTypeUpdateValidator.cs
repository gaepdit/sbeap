using FluentValidation;
using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.Domain.Entities.EntityBase;

namespace Sbeap.AppServices.ActionItemTypes.Validators;

public class ActionItemTypeUpdateValidator : AbstractValidator<ActionItemTypeUpdateDto>
{
    private readonly IActionItemTypeRepository _repository;

    public ActionItemTypeUpdateValidator(IActionItemTypeRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(SimpleNamedEntity.MinNameLength, SimpleNamedEntity.MaxNameLength)
            .MustAsync(async (e, _, token) => await NotDuplicateName(e, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(SimpleNamedEntityUpdateDto item, CancellationToken token = default)
    {
        var existing = await _repository.FindByNameAsync(item.Name, token);
        return existing is null || existing.Id == item.Id;
    }
}