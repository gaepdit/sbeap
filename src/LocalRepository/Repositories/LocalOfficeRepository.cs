﻿using Sbeap.Domain.Identity;
using Sbeap.Domain.Offices;
using Sbeap.TestData.Offices;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalOfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public LocalOfficeRepository() : base(OfficeData.GetOffices) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Task.FromResult(Items.SingleOrDefault(e => e.Name == name));

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
}
