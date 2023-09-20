using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalOfficeRepository : NamedEntityRepository<Office>, IOfficeRepository
{
    public LocalOfficeRepository() : base(OfficeData.GetOffices) { }

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList();
}
