using GaEpd.AppLibrary.Domain.Repositories;
using Sbeap.Domain.Entities.SicCodes;

namespace Sbeap.EfRepository.Repositories;

public sealed class SicRepository(AppDbContext context) : ISicRepository
{
    public Task<bool> ExistsAsync(string id, CancellationToken token = default) =>
        context.Set<SicCode>().AnyAsync(e => e.Id == id, token);

    public async Task<SicCode> GetAsync(string id, CancellationToken token = default) =>
        await context.Set<SicCode>().SingleOrDefaultAsync(sic => sic.Id.Equals(id), token).ConfigureAwait(false)
        ?? throw new EntityNotFoundException<SicCode>(id);

    public async Task<IReadOnlyCollection<SicCode>> GetListAsync(CancellationToken token = default) =>
        await context.Set<SicCode>().AsNoTracking().Where(sic => sic.Active).OrderBy(sic => sic.Id).ToListAsync(token)
            .ConfigureAwait(false);

    public void Dispose() => context.Dispose();
    public async ValueTask DisposeAsync() => await context.DisposeAsync().ConfigureAwait(false);
}
