using Sbeap.Domain.Entities.ActionItems;
using Sbeap.TestData;

namespace Sbeap.LocalRepository.Repositories;

public sealed class LocalActionItemRepository()
    : BaseRepository<ActionItem, Guid>(ActionItemData.GetActionItems), IActionItemRepository;
