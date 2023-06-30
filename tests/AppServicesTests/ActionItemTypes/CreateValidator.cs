using FluentValidation.TestHelper;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.ActionItemTypes.Validators;
using Sbeap.TestData.Constants;
using Sbeap.Domain.Entities.ActionItemTypes;

namespace AppServicesTests.ActionItemTypes;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionItemType?)null);
        var model = new ActionItemTypeCreateDto(TextData.ValidName);

        var validator = new ActionItemTypeCreateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ActionItemType(Guid.Empty, TextData.ValidName));
        var model = new ActionItemTypeCreateDto(TextData.ValidName);

        var validator = new ActionItemTypeCreateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionItemType?)null);
        var model = new ActionItemTypeCreateDto(TextData.ShortName);

        var validator = new ActionItemTypeCreateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
