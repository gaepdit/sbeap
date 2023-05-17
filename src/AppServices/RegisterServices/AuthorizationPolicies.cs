﻿using Cts.AppServices.Complaints.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Sbeap.AppServices.Permissions;

namespace Sbeap.AppServices.RegisterServices;

public static class AuthorizationPolicies
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(PolicyName.AdminUser, Policies.AdminUserPolicy());
            opts.AddPolicy(PolicyName.StaffUser, Policies.StaffUserPolicy());
            opts.AddPolicy(PolicyName.SiteMaintainer, Policies.SiteMaintainerPolicy());
            opts.AddPolicy(PolicyName.UserAdministrator, Policies.UserAdministratorPolicy());
        });

        services.AddSingleton<IAuthorizationHandler>(_ => new CustomerViewPermissionsHandler());
    }
}
