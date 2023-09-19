using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;

namespace Sbeap.EfRepository.Repositories;

public sealed class OfficeRepository : NamedEntityRepository<Office>, IOfficeRepository
{
    public OfficeRepository(DbContext context) : base(context) { }

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList();
}
