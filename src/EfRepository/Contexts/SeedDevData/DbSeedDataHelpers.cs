using Microsoft.AspNetCore.Identity;
using Sbeap.Domain.Data;
using Sbeap.Domain.Identity;
using Sbeap.TestData;
using Sbeap.TestData.Identity;

namespace Sbeap.EfRepository.Contexts.SeedDevData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        SeedSicData(context);
        SeedIdentityData(context);
        SeedActionItemTypeData(context);
        SeedAgencyData(context);
        SeedCustomerData(context);
        SeedContactData(context);
        SeedCaseworkData(context);
        SeedActionItemData(context);
    }

    private static void SeedActionItemData(AppDbContext context)
    {
        if (context.ActionItems.Any()) return;
        context.ActionItems.AddRange(ActionItemData.GetActionItems);
        context.SaveChanges();
    }

    internal static void SeedActionItemTypeData(AppDbContext context)
    {
        if (context.ActionItemTypes.Any()) return;
        context.ActionItemTypes.AddRange(ActionItemTypeData.GetActionItemTypes);
        context.SaveChanges();
    }

    internal static void SeedAgencyData(AppDbContext context)
    {
        if (context.Agencies.Any()) return;
        context.Agencies.AddRange(AgencyData.GetAgencies);
        context.SaveChanges();
    }

    private static void SeedCaseworkData(AppDbContext context)
    {
        if (context.Cases.Any()) return;
        context.Cases.AddRange(CaseworkData.GetCases);
        context.SaveChanges();
    }

    internal static void SeedContactData(AppDbContext context)
    {
        if (context.Contacts.Any()) return;
        context.Contacts.AddRange(ContactData.GetContacts(false));
        context.SaveChanges();
    }

    private static void SeedCustomerData(AppDbContext context)
    {
        if (context.Customers.Any()) return;
        context.Customers.AddRange(CustomerData.GetCustomers);
        context.SaveChanges();
    }

    internal static void SeedOfficeData(AppDbContext context)
    {
        if (context.Offices.Any()) return;
        context.Offices.AddRange(OfficeData.GetOffices);
        context.SaveChanges();
    }

    private static void SeedSicData(AppDbContext context)
    {
        if (context.SicCodes.Any()) return;
        context.SicCodes.AddRange(SicCodeData.GetSicCodes);
        context.SaveChanges();
    }

    private static void SeedIdentityData(AppDbContext context)
    {
        // Seed Users
        var users = UserData.GetUsers.ToList();
        if (!context.Users.Any()) context.Users.AddRange(users);

        // Seed Roles
        var roles = RoleData.GetRoles.ToList();
        if (!context.Roles.Any()) context.Roles.AddRange(roles);

        // Seed User Roles
        if (!context.UserRoles.Any())
        {
            // -- admin
            var adminUserRoles = roles
                .Select(role => new IdentityUserRole<string>
                    { RoleId = role.Id, UserId = users.Single(e => e.GivenName == "Admin").Id })
                .ToList();
            context.UserRoles.AddRange(adminUserRoles);

            // -- staff
            var staffUserId = users.Single(e => e.GivenName == "General").Id;
            context.UserRoles.AddRange(
                new IdentityUserRole<string>
                {
                    RoleId = roles.Single(e => e.Name == RoleName.SiteMaintenance).Id,
                    UserId = staffUserId,
                },
                new IdentityUserRole<string>
                {
                    RoleId = roles.Single(e => e.Name == RoleName.Staff).Id,
                    UserId = staffUserId,
                });
        }

        context.SaveChanges();
    }
}
