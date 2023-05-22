using Sbeap.LocalRepository.Repositories;

namespace LocalRepositoryTests.Customers;

public class FindIncludeAll
{
    private LocalCustomerRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetCustomerRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        item.Contacts.RemoveAll(c => c.IsDeleted);

        var result = await _repository.FindIncludeAllAsync(item.Id, true);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindIncludeAllAsync(Guid.Empty, true);
        result.Should().BeNull();
    }
}
