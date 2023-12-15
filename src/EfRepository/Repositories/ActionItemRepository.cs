using Sbeap.Domain.Entities.ActionItems;

namespace Sbeap.EfRepository.Repositories;

public sealed class ActionItemRepository(AppDbContext context)
    : BaseRepository<ActionItem, Guid, AppDbContext>(context), IActionItemRepository;
