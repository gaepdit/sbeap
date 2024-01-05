using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Sbeap.AppServices.Customers.Permissions;

public class CustomerOperation :
    OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private CustomerOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<CustomerOperation> AllOperations { get; } = [];

    public static readonly CustomerOperation Edit = new(nameof(Edit));
    public static readonly CustomerOperation ManageDeletions = new(nameof(ManageDeletions));
}
