using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;

namespace Sbeap.EfRepository.Repositories;

public sealed class OfficeRepository(AppDbContext context)
    : NamedEntityRepository<Office, AppDbContext>(context), IOfficeRepository
{
    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token).ConfigureAwait(false)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList();
}
