using Sbeap.Domain.Exceptions;

namespace Sbeap.Domain.Entities.Agencies;

/// <inheritdoc />
public class AgencyManager : IAgencyManager
{
    private readonly IAgencyRepository _repository;
    public AgencyManager(IAgencyRepository repository) => _repository = repository;

    public async Task<Agency> CreateAsync(string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, ignoreId: null, token);
        return new Agency(Guid.NewGuid(), name);
    }

    public async Task ChangeNameAsync(Agency agency, string name, CancellationToken token = default)
    {
        await ThrowIfDuplicateName(name, agency.Id, token);
        agency.ChangeName(name);
    }

    private async Task ThrowIfDuplicateName(string name, Guid? ignoreId, CancellationToken token)
    {
        // Validate the name is not a duplicate.
        var existing = await _repository.FindByNameAsync(name.Trim(), token);
        if (existing is not null && (ignoreId is null || existing.Id != ignoreId))
            throw new NameAlreadyExistsException(name);
    }
}
