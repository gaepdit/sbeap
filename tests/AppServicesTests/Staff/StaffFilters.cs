﻿using MyAppRoot.AppServices.Staff;
using MyAppRoot.TestData.Identity;

namespace AppServicesTests.Staff;

public class StaffFilters
{
    [Test]
    public void DefaultFilter_ReturnsAllActive()
    {
        var filter = new StaffSearchDto();
        var expected = UserData.GetUsers.Where(e => e.Active);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NameFilter_ReturnsMatches()
    {
        var name = UserData.GetUsers.First(e => e.Active).GivenName;
        var filter = new StaffSearchDto { Name = name };
        var expected = UserData.GetUsers
            .Where(e => e.Active &&
                (string.Equals(e.GivenName, name, StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(e.FamilyName, name, StringComparison.CurrentCultureIgnoreCase)));

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EmailFilter_ReturnsMatches()
    {
        var email = UserData.GetUsers.First(e => e.Active).Email;
        var filter = new StaffSearchDto { Email = email };
        var expected = UserData.GetUsers
            .Where(e => e.Active && e.Email == email);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void OfficeFilter_ReturnsMatches()
    {
        var office = UserData.GetUsers.First(e => e is { Active: true, Office: { } }).Office;
        var filter = new StaffSearchDto { Office = office!.Id };
        var expected = UserData.GetUsers
            .Where(e => e.Active && e.Office == office);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void InactiveFilter_ReturnsAllInactive()
    {
        var filter = new StaffSearchDto { Status = StaffSearchDto.ActiveStatus.Inactive };
        var expected = UserData.GetUsers.Where(e => !e.Active);

        var result = UserData.GetUsers.AsQueryable().ApplyFilter(filter);

        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StatusAllFilter_ReturnsAll()
    {
        var filter = new StaffSearchDto { Status = StaffSearchDto.ActiveStatus.All };
        var result = UserData.GetUsers.AsQueryable().ApplyFilter(filter);
        result.Should().BeEquivalentTo(UserData.GetUsers);
    }
}
