using Sbeap.Domain.Entities.EntityBase;
using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Exceptions;
using Sbeap.TestData.Constants;

namespace DomainTests.Offices.Manager;

public class ChangeName
{
    [Test]
    public async Task WhenNewNameIsValid_ChangesName()
    {
        var item = new Office(Guid.Empty, TextData.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TextData.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var manager = new OfficeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, TextData.NewValidName);

        item.Name.Should().BeEquivalentTo(TextData.NewValidName);
    }

    [Test]
    public async Task WhenNewNameIsUnchanged_CompletesWithNoChange()
    {
        var item = new Office(Guid.Empty, TextData.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TextData.ValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);
        var manager = new OfficeManager(repoMock.Object);

        await manager.ChangeNameAsync(item, TextData.ValidName);

        item.Name.Should().BeEquivalentTo(TextData.ValidName);
    }

    [Test]
    public async Task WhenNewNameAlreadyExists_Throws()
    {
        var item = new Office(Guid.Empty, TextData.ValidName);
        var existingItem = new Office(Guid.NewGuid(), TextData.NewValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TextData.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingItem);
        var manager = new OfficeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, TextData.NewValidName);

        (await action.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TextData.NewValidName}");
    }

    [Test]
    public async Task WhenNewNameIsInvalid_Throws()
    {
        var item = new Office(Guid.Empty, TextData.ValidName);
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(TextData.NewValidName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var manager = new OfficeManager(repoMock.Object);

        var action = async () => await manager.ChangeNameAsync(item, TextData.ShortName);

        (await action.Should().ThrowAsync<ArgumentException>())
            .WithMessage($"The length must be at least the minimum length '{SimpleNamedEntity.MinNameLength}'.*");
    }
}
