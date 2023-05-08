using FluentValidation.TestHelper;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Offices.Validators;
using Sbeap.Domain.Entities.Offices;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Offices;

public class UpdateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var model = new OfficeUpdateDto
        {
            Id = Guid.Empty,
            Name = TextData.ValidName,
            Active = true,
        };

        var validator = new OfficeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Office(Guid.NewGuid(), TextData.ValidName));
        var model = new OfficeUpdateDto
        {
            Id = Guid.Empty,
            Name = TextData.ValidName,
            Active = true,
        };

        var validator = new OfficeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Office(Guid.Empty, TextData.ValidName));
        var model = new OfficeUpdateDto
        {
            Id = Guid.Empty,
            Name = TextData.ValidName,
            Active = true,
        };

        var validator = new OfficeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IOfficeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Office?)null);
        var model = new OfficeUpdateDto() { Name = TextData.ShortName };

        var validator = new OfficeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
