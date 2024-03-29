﻿using Sbeap.Domain.Identity;

namespace Sbeap.Domain.Entities.Offices;

public interface IOfficeRepository : INamedEntityRepository<Office>
{
    /// <summary>
    /// Returns a list of all active <see cref="ApplicationUser"/> located in the <see cref="Office"/> with the
    /// given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ID of the Office.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <exception cref="EntityNotFoundException{Office}">Thrown if no entity exists with the given Id.</exception>
    /// <returns>A list of Users.</returns>
    Task<List<ApplicationUser>> GetActiveStaffMembersListAsync(Guid id, CancellationToken token = default);
}
