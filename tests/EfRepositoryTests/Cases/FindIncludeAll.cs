using Sbeap.Domain.Entities.Cases;
using Sbeap.TestData;

namespace EfRepositoryTests.Cases;

public class FindIncludeAll
{
    private ICaseworkRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateSqlServerRepositoryHelper(this).GetCaseworkRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = CaseworkData.GetCases.First();
        item.ActionItems.RemoveAll(e => e.IsDeleted);
        var result = await _repository.FindIncludeAllAsync(item.Id);
        result.Should().BeEquivalentTo(item, opts => opts
            .IgnoringCyclicReferences()
            .Excluding(e => e.Customer.Contacts)
            .Excluding(e => e.Customer.Cases)
            .For(e => e.ActionItems).Exclude(i => i.EnteredBy)
        );
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindIncludeAllAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
