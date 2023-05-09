using Sbeap.Domain.Entities.Customers;
using Sbeap.TestData;

namespace EfRepositoryTests.Customers;

public class FindIncludeAll
{
    private ICustomerRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetCustomerRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = CustomerData.GetCustomers.First();
        var result = await _repository.FindIncludeAllAsync(item.Id);
        result.Should().BeEquivalentTo(item, opts => opts
            .IgnoringCyclicReferences()
            .For(e => e.Cases).Exclude(i => i.ActionItems)
        );
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindIncludeAllAsync(Guid.Empty);
        result.Should().BeNull();
    }
}
