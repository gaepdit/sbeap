using GaEpd.AppLibrary.Domain.Repositories;
using Sbeap.Domain.Entities.SicCodes;

namespace Sbeap.EfRepository.Repositories;

public sealed class SicRepository : ISicRepository
{
    private readonly AppDbContext _context;
    public SicRepository(AppDbContext context) => _context = context;

    public async Task<SicCode> GetAsync(string id, CancellationToken token = default) =>
        await _context.Set<SicCode>().SingleOrDefaultAsync(sic => sic.Id.Equals(id), token)
        ?? throw new EntityNotFoundException(typeof(SicCode), id);

    public async Task<IReadOnlyCollection<SicCode>> GetListAsync(CancellationToken token = default) =>
        await _context.Set<SicCode>().AsNoTracking().Where(sic => sic.Active).OrderBy(sic => sic.Id).ToListAsync(token);

    public void Dispose() => _context.Dispose();
    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
