using Microsoft.AspNetCore.Identity;
using Sbeap.TestData;
using Sbeap.TestData.Identity;

namespace Sbeap.EfRepository.Contexts.SeedDevData;

public static class DbSeedDataHelpers
{
    public static void SeedAllData(AppDbContext context)
    {
        SeedOfficeData(context);
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

    private static void SeedActionItemTypeData(AppDbContext context)
    {
        if (context.ActionItemTypes.Any()) return;
        context.ActionItemTypes.AddRange(ActionItemTypeData.GetActionItemTypes);
        context.SaveChanges();
    }

    private static void SeedAgencyData(AppDbContext context)
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

    private static void SeedContactData(AppDbContext context)
    {
        if (context.Contacts.Any()) return;
        context.Contacts.AddRange(ContactData.GetContacts);
        context.SaveChanges();
    }

    private static void SeedCustomerData(AppDbContext context)
    {
        if (context.Customers.Any()) return;
        context.Customers.AddRange(CustomerData.GetCustomers);
        context.SaveChanges();
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
