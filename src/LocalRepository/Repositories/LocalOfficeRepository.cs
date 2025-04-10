using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalOfficeRepository() : NamedEntityRepository<Office>(OfficeData.GetOffices), IOfficeRepository
{
    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token: token)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList();
}
