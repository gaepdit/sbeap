using Sbeap.TestData.Constants;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Agencies.Validators;
using FluentValidation.TestHelper;

namespace AppServicesTests.Agencies;

internal class UpdateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = new Mock<IAgencyRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Agency?)null);
        var model = new AgencyUpdateDto(Id: Guid.Empty, Name: TextData.ValidName, Active: true);

        var validator = new AgencyUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IAgencyRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Agency(Guid.NewGuid(), TextData.ValidName));
        var model = new AgencyUpdateDto(Id: Guid.Empty, Name: TextData.ValidName, Active: true);

        var validator = new AgencyUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = new Mock<IAgencyRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Agency(Guid.Empty, TextData.ValidName));
        var model = new AgencyUpdateDto(Id: Guid.Empty, Name: TextData.ValidName, Active: true);

        var validator = new AgencyUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IAgencyRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Agency?)null);
        var model = new AgencyUpdateDto(Id: Guid.Empty, Name: TextData.ShortName, Active: true);

        var validator = new AgencyUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
