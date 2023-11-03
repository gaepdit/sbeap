using GaEpd.AppLibrary.ListItems;
using Sbeap.Domain.Entities.SicCodes;

namespace Sbeap.AppServices.SicCodes;

public sealed class SicService : ISicService
{
    private readonly ISicRepository _repository;
    public SicService(ISicRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<ListItem<string>>> GetActiveListItemsAsync(CancellationToken token = default) =>
        (await _repository.GetListAsync(token)).Select(sic => new ListItem<string>(sic.Id, Name: sic.Display)).ToList();

    void IDisposable.Dispose() => _repository.Dispose();
}
