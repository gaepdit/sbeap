using MyAppRoot.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadRepository;

public class ExistsByPredicate
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsTrue()
    {
        var item = _repository.Items.First();
        var result = await _repository.ExistsAsync(e => e.Id == item.Id);
        result.Should().BeTrue();
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.ExistsAsync(e => e.Id == Guid.Empty);
        result.Should().BeFalse();
    }
}
