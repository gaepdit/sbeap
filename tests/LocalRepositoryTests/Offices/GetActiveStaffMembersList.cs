using GaEpd.AppLibrary.Domain.Repositories;
using Sbeap.Domain.Entities.Offices;
using Sbeap.LocalRepository.Repositories;

namespace LocalRepositoryTests.Offices;

public class GetActiveStaffMembersList
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetOfficeRepository();

    [TearDown]
    public async Task TearDown() => await _repository.DisposeAsync();

    [Test]
    public async Task WhenStaffExist_ReturnsList()
    {
        var item = _repository.Items.First(e => e.Active);
        var result = await _repository.GetActiveStaffMembersListAsync(item.Id);
        result.Should().BeEquivalentTo(item.StaffMembers);
    }

    [Test]
    public async Task WhenStaffDoNotExist_ReturnsEmptyList()
    {
        var item = _repository.Items.Last();
        var result = await _repository.GetActiveStaffMembersListAsync(item.Id);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task WhenOfficeDoesNotExist_Throws()
    {
        var id = Guid.Empty;
        var action = async () => await _repository.GetActiveStaffMembersListAsync(id);
        (await action.Should().ThrowAsync<EntityNotFoundException<Office>>())
            .WithMessage($"Entity not found. Entity type: {typeof(Office).FullName}, id: {id}");
    }
}
