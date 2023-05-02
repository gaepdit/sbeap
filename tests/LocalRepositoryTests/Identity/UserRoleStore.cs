using FluentAssertions.Execution;
using Sbeap.LocalRepository.Identity;
using System.Diagnostics;

namespace LocalRepositoryTests.Identity;

public class UserRoleStore
{
    private LocalUserStore _store = default!;

    [SetUp]
    public void SetUp() => _store = RepositoryHelper.GetLocalUserStore();

    [TearDown]
    public void TearDown() => _store.Dispose();

    [Test]
    public async Task AddToRole_AddsRole()
    {
        var user = _store.UserStore.Last();
        var roleName = _store.Roles.First().Name;
        Debug.Assert(roleName != null, "role.NormalizedName != null");
        var resultBefore = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        await _store.AddToRoleAsync(user, roleName, CancellationToken.None);
        var resultAfter = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        using (new AssertionScope())
        {
            resultBefore.Should().BeFalse();
            resultAfter.Should().BeTrue();
        }
    }

    [Test]
    public async Task RemoveFromRole_RemovesRole()
    {
        var user = _store.UserStore.First();
        var roleName = _store.Roles.First().Name;
        Debug.Assert(roleName != null, "role.NormalizedName != null");
        var resultBefore = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        await _store.RemoveFromRoleAsync(user, roleName, CancellationToken.None);
        var resultAfter = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);

        using (new AssertionScope())
        {
            resultBefore.Should().BeTrue();
            resultAfter.Should().BeFalse();
        }
    }

    [Test]
    public async Task GetRoles_ReturnsListOfRoles()
    {
        var user = _store.UserStore.First();

        var result = await _store.GetRolesAsync(user, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(_store.Roles.Count);
        }
    }

    [Test]
    public async Task GetRoles_IfNone_ReturnsEmptyList()
    {
        var user = _store.UserStore.Last();

        var result = await _store.GetRolesAsync(user, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }
    }

    [Test]
    public async Task IsInRole_IfSo_ReturnsTrue()
    {
        var user = _store.UserStore.First();
        var roleName = _store.Roles.First().Name;
        Debug.Assert(roleName != null, "role.NormalizedName != null");
        var result = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);
        result.Should().BeTrue();
    }

    [Test]
    public async Task IsInRole_IfNot_ReturnsFalse()
    {
        var user = _store.UserStore.Last();
        var roleName = _store.Roles.First().Name;
        Debug.Assert(roleName != null, "role.NormalizedName != null");
        var result = await _store.IsInRoleAsync(user, roleName, CancellationToken.None);
        result.Should().BeFalse();
    }

    [Test]
    public async Task GetUsersInRole_IfSome_ReturnsListOfUsers()
    {
        var roleName = _store.Roles.First().Name;
        Debug.Assert(roleName != null, "role.NormalizedName != null");
        var result = await _store.GetUsersInRoleAsync(roleName, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().HaveCount(1);
            result[0].Should().Be(_store.UserStore.First());
        }
    }

    [Test]
    public async Task GetUsersInRole_IfNone_ReturnsEmptyList()
    {
        var result = await _store.GetUsersInRoleAsync("None", CancellationToken.None);
        result.Should().HaveCount(0);
    }
}
