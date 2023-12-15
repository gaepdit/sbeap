using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.ActionItemTypes.Validators;

public class ActionItemTypeCreateValidator(IActionItemTypeRepository repository) :
    StandardNamedEntityCreateValidator<ActionItemType, ActionItemTypeCreateDto, IActionItemTypeRepository>(repository);
