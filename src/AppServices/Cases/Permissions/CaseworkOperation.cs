using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Sbeap.AppServices.Cases.Permissions;

public class CaseworkOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private CaseworkOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<CaseworkOperation> AllOperations { get; } = [];

    public static readonly CaseworkOperation Edit = new(nameof(Edit));
    public static readonly CaseworkOperation EditActionItems = new(nameof(EditActionItems));
    public static readonly CaseworkOperation ManageDeletions = new(nameof(ManageDeletions));
}
