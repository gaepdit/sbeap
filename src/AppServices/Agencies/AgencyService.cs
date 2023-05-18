using GaEpd.AppLibrary.ListItems;
using Sbeap.Domain.Entities.Agencies;

namespace Sbeap.AppServices.Agencies;

public sealed class AgencyService : IAgencyService
{
    private readonly IAgencyRepository _repository;
    public AgencyService(IAgencyRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<ListItem>> GetListItemsAsync(
        bool activeOnly = true, CancellationToken token = default) =>
        (await _repository.GetListAsync(e => !activeOnly || e.Active, token)).OrderBy(e => e.Name)
        .Select(e => new ListItem(e.Id, e.NameWithActivity)).ToList();

    public void Dispose() => _repository.Dispose();
}
