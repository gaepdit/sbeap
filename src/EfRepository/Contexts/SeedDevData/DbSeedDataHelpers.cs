using Microsoft.AspNetCore.Identity;
using MyAppRoot.TestData;
using MyAppRoot.TestData.Identity;

namespace MyAppRoot.EfRepository.Contexts.SeedDevData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        SeedOfficeData(context);
        SeedIdentityData(context);
    }

    public static void SeedOfficeData(AppDbContext context)
    {
        if (context.Offices.Any()) return;
        context.Offices.AddRange(OfficeData.GetOffices);
        context.SaveChanges();
    }

    private static void SeedIdentityData(AppDbContext context)
    {
        var roles = UserData.GetRoles.ToList();
        var users = UserData.GetUsers;
        var userRoles = roles
            .Select(role => new IdentityUserRole<string> { RoleId = role.Id, UserId = users.First().Id })
            .ToList();

        if (!context.Roles.Any()) context.Roles.AddRange(roles);
        if (!context.Users.Any()) context.Users.AddRange(users);
        if (!context.UserRoles.Any()) context.UserRoles.AddRange(userRoles);

        context.SaveChanges();
    }
}
