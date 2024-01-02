using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.EfRepository.Repositories;

public sealed class ActionItemTypeRepository(AppDbContext context)
    : NamedEntityRepository<ActionItemType, AppDbContext>(context), IActionItemTypeRepository;
