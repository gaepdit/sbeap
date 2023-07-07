using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Sbeap.AppServices.Agencies.Permissions;

public class AgencyOperation : OperationAuthorizationRequirement // implements IAuthorizationRequirement
{
    private AgencyOperation(string name)
    {
        Name = name;
        AllOperations.Add(this);
    }

    public static List<AgencyOperation> AllOperations { get; } = new();

    public static readonly AgencyOperation Edit = new(nameof(Edit));

    public static readonly AgencyOperation ManageDeletions = new(nameof(ManageDeletions));
}