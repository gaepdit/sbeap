using Sbeap.Domain.Entities.Agencies;
using Sbeap.TestData;
using Sbeap.TestData.Constants;

namespace EfRepositoryTests.Agencies;

public class FindByName
{
    private IAgencyRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetAgencyRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = AgencyData.GetAgencies.First();
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
