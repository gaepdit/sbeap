using FluentAssertions.Execution;
using Sbeap.Domain.Offices;

namespace DomainTests.Offices.Entity;

public class Constructor
{
    [Test]
    public void WithValidInput_ReturnsNewEntity()
    {
        var newGuid = Guid.NewGuid();
        var result = new Office(newGuid, newGuid.ToString());
        using (new AssertionScope())
        {
            result.Id.Should().Be(newGuid);
            result.Name.Should().Be(newGuid.ToString());
            result.Active.Should().BeTrue();
        }
    }

    [Test]
    public void WithEmptyName_Throws()
    {
        var action = () => new Office(Guid.Empty, string.Empty);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null, empty, or white space.*");
    }

    [Test]
    public void WithShortName_Throws()
    {
        var action = () => new Office(Guid.Empty, "a");
        action.Should().Throw<ArgumentException>()
            .WithMessage($"The length must be at least the minimum length '{Office.MinNameLength}'.*");
    }
}
