using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Sbeap.AppServices.ActionItemTypes.Permissions;

public class ActionItemTypeOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private ActionItemTypeOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<ActionItemTypeOperation> AllOperations { get; } = new();

    public static readonly ActionItemTypeOperation Edit = new(nameof(Edit));
    public static readonly ActionItemTypeOperation ManageDeletions = new(nameof(ManageDeletions));
}