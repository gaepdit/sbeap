using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData;
using Sbeap.TestData.Constants;

namespace EfRepositoryTests.ActionItemTypes;

public class FindByName
{
    private IActionItemTypeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetActionItemTypeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public async Task WhenItemExists_ReturnsItem()
    {
        var item = ActionItemTypeData.GetActionItemTypes.First(e => e.Active);
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
