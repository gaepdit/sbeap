using Sbeap.LocalRepository.Repositories;

namespace LocalRepositoryTests.Cases;

public class FindIncludeAll
{
    private LocalCaseworkRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.GetCaseworkRepository();

    [TearDown]
    public async Task TearDown() => await _repository.DisposeAsync();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = _repository.Items.First();
        var result = await _repository.FindIncludeAllAsync(item.Id);
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindIncludeAllAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
