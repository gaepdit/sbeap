﻿using Microsoft.AspNetCore.Identity;
using MyAppRoot.Domain.Identity;

namespace MyAppRoot.TestData.Identity;

internal static partial class UserData
{
    private static IEnumerable<IdentityRole>? _roles;

    public static IEnumerable<IdentityRole> GetRoles
    {
        get
        {
            if (_roles is not null) return _roles;
            _roles = AppRole.AllRoles
                .Select(r => new IdentityRole(r.Value.Name) { NormalizedName = r.Key.ToUpperInvariant() })
                .ToList();
            return _roles;
        }
    }
}
