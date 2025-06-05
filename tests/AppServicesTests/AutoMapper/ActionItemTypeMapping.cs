using Sbeap.AppServices.ActionItemTypes;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData.Constants;

namespace AppServicesTests.AutoMapper;

internal class ActionItemTypeMapping
{
    [Test]
    public void ActionItemTypeViewMappingWorks()
    {
        var item = new ActionItemType(Guid.NewGuid(), TextData.ValidName);

        var result = AppServicesTestsSetup.Mapper!.Map<ActionItemTypeViewDto>(item);

        using (new AssertionScope())
        {
            result.Id.Should().Be(item.Id);
            result.Name.Should().Be(item.Name);
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void ActionItemTypeUpdateMappingWorks()
    {
        var item = new ActionItemType(Guid.NewGuid(), TextData.ValidName);

        var result = AppServicesTestsSetup.Mapper!.Map<ActionItemTypeUpdateDto>(item);

        using (new AssertionScope())
        {
            result.Name.Should().Be(item.Name);
            result.Active.Should().BeTrue();
        }
    }
}
