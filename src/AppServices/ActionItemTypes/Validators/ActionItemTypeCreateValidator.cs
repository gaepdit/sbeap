using Sbeap.AppServices.DtoBase;
using Sbeap.Domain.Entities.ActionItemTypes;

namespace Sbeap.AppServices.ActionItemTypes.Validators;

public class ActionItemTypeCreateValidator :
    StandardNamedEntityCreateValidator<ActionItemType, ActionItemTypeCreateDto, IActionItemTypeRepository>
{
    public ActionItemTypeCreateValidator(IActionItemTypeRepository repository) : base(repository) { }
}
