using Sbeap.LocalRepository.Repositories;
using Sbeap.TestData.Constants;

namespace LocalRepositoryTests.Agencies;

public class FindByName
{
    private LocalAgencyRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetAgencyRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        var result = await _repository.FindByNameAsync(item.Name);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindByNameAsync(TextData.NonExistentName);
        result.Should().BeNull();
    }
}
