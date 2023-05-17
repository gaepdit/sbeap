using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Sbeap.AppServices.Customers.Permissions;

public static class CustomerOperationNames
{
    public const string ManageDeletions = nameof(ManageDeletions);
}

public class CustomerOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private CustomerOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<CustomerOperation> AllOperations { get; } = new();

    public static readonly CustomerOperation ManageDeletions = new(CustomerOperationNames.ManageDeletions);
}
