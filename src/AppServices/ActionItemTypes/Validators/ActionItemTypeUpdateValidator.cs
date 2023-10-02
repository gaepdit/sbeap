using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.ActionItemTypes.Validators;

public class ActionItemTypeUpdateValidator :
    StandardNamedEntityUpdateValidator<ActionItemType, ActionItemTypeUpdateDto, IActionItemTypeRepository>
{
    public ActionItemTypeUpdateValidator(IActionItemTypeRepository repository) : base(repository) { }
}
