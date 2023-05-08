using Sbeap.Domain.Entities.Offices;
using Sbeap.Domain.Exceptions;
using Sbeap.TestData.Constants;

namespace DomainTests.Offices.Manager;

public class Create
{
    [Test]
    public async Task WhenItemIsValid_CreatesItem()
    {
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var manager = new OfficeManager(repoMock.Object);

        var newItem = await manager.CreateAsync(TextData.ValidName);

        newItem.Name.Should().BeEquivalentTo(TextData.ValidName);
    }

    [Test]
    public async Task WhenItemIsInvalid_Throws()
    {
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Office(Guid.Empty, TextData.ValidName));
        var manager = new OfficeManager(repoMock.Object);

        var office = async () => await manager.CreateAsync(TextData.ValidName);

        (await office.Should().ThrowAsync<NameAlreadyExistsException>())
            .WithMessage($"An entity with that name already exists. Name: {TextData.ValidName}");
    }
}
