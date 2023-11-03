using GaEpd.AppLibrary.Domain.Repositories;
using Sbeap.Domain.Data;
using Sbeap.Domain.Entities.SicCodes;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalSicRepository : ISicRepository
{
    public Task<SicCode> GetAsync(string id, CancellationToken token = default) =>
        SicCodeData.GetSicCodes.Any(sic => sic.Id.Equals(id))
            ? Task.FromResult(SicCodeData.GetSicCodes.Single(sic => sic.Id.Equals(id)))
            : throw new EntityNotFoundException(typeof(SicCode), id);

    public Task<IReadOnlyCollection<SicCode>> GetListAsync(CancellationToken token = default) =>
        Task.FromResult(
            SicCodeData.GetSicCodes.Where(sic => sic.Active).OrderBy(sic => sic.Id).ToList() as
                IReadOnlyCollection<SicCode>);

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
