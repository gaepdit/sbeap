using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Identity;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalOfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public LocalOfficeRepository() : base(OfficeData.GetOffices) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => string.Equals(e.Name.ToUpper(), name.ToUpper())));

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.FamilyName).ThenBy(e => e.GivenName).ToList();
}
