using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.ActionItemTypes.Validators;

public class ActionItemTypeUpdateValidator(IActionItemTypeRepository repository) :
    StandardNamedEntityUpdateValidator<ActionItemType, ActionItemTypeUpdateDto, IActionItemTypeRepository>(repository);
