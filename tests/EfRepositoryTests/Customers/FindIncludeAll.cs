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
        item.Contacts.RemoveAll(c => c.IsDeleted);

        var result = await _repository.FindIncludeAllAsync(item.Id, true);

        result.Should().BeEquivalentTo(item, opts => opts
            .IgnoringCyclicReferences()
            .For(e => e.Cases).Exclude(i => i.ActionItems)
            .For(e => e.Contacts).Exclude(i => i.EnteredBy)
        );
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var result = await _repository.FindIncludeAllAsync(Guid.Empty, true);
        result.Should().BeNull();
    }
}
