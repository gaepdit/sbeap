using FluentAssertions.Execution;
using Sbeap.LocalRepository.Repositories;
using Sbeap.TestData.Constants;

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
        var result = await _repository.GetListAsync(e => e.Name == item.Name);
        using (new AssertionScope())
        {
            result.Count.Should().Be(1);
            result.First().Should().BeEquivalentTo(item);
        }
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsEmptyList()
    {
        var result = await _repository.GetListAsync(e => e.Name == TestConstants.NonExistentName);
        result.Should().BeEmpty();
    }
}
