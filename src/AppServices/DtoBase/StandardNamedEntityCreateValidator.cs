using FluentValidation;
using GaEpd.AppLibrary.Domain.Entities;
using GaEpd.AppLibrary.Domain.Repositories;
using Sbeap.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Sbeap.AppServices.DtoBase;

[SuppressMessage("", "S2436")]
public abstract class StandardNamedEntityCreateValidator<TEntity, TDto, TRepository> : AbstractValidator<TDto>
    where TEntity : IEntity<Guid>, INamedEntity
    where TDto : INamedEntity
    where TRepository : INamedEntityRepository<TEntity>
{
    private readonly TRepository _repository;

    protected StandardNamedEntityCreateValidator(TRepository repository)
    {
        _repository = repository;

        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(AppConstants.MinimumNameLength, AppConstants.MaximumNameLength)
            .MustAsync(async (name, token) => await NotDuplicateName(name, token))
            .WithMessage("The name entered already exists.");
    }

    private async Task<bool> NotDuplicateName(string name, CancellationToken token = default) =>
        await _repository.FindByNameAsync(name, token) is null;
}
