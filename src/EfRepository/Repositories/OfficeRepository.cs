﻿using Microsoft.EntityFrameworkCore;
using MyAppRoot.Domain.Identity;
using MyAppRoot.Domain.Offices;
using MyAppRoot.EfRepository.Contexts;

namespace MyAppRoot.EfRepository.Repositories;

public sealed class OfficeRepository : BaseRepository<Office, Guid>, IOfficeRepository
{
    public OfficeRepository(AppDbContext context) : base(context) { }

    public Task<Office?> FindByNameAsync(string name, CancellationToken token = default) =>
        Context.Offices.AsNoTracking()
            .SingleOrDefaultAsync(e => string.Equals(e.Name.ToUpper(), name.ToUpper()), token);

    public async Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(
        Guid id, CancellationToken token = default) =>
        (await GetAsync(id, token)).StaffMembers
        .Where(e => e.Active)
        .OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
}
