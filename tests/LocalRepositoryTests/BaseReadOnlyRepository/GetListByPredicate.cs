using FluentAssertions.Execution;
using MyAppRoot.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadOnlyRepository;

public class GetListByPredicate
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemsExist_ReturnsList()
    {
        var item = _repository.Items.First();
        var result = await _repository.GetListAsync(e => e.Id == item.Id);
        using (new AssertionScope())
        {
            result.Count.Should().Be(1);
            result.First().Should().BeEquivalentTo(item);
        }
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetListAsync(e => e.Id == Guid.Empty);
        result.Should().BeEmpty();
    }
}
