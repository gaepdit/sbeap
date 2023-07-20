using FluentValidation.TestHelper;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.ActionItemTypes.Validators;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData.Constants;

namespace AppServicesTests.ActionItemTypes;

internal class UpdateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionItemType?)null);
        var model = new ActionItemTypeUpdateDto(Id: Guid.Empty, Name: TextData.ValidName, Active: true);

        var validator = new ActionItemTypeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ActionItemType(Guid.NewGuid(), TextData.ValidName));
        var model = new ActionItemTypeUpdateDto(Id: Guid.Empty, Name: TextData.ValidName, Active: true);

        var validator = new ActionItemTypeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ActionItemType(Guid.Empty, TextData.ValidName));
        var model = new ActionItemTypeUpdateDto(Id: Guid.Empty, Name: TextData.ValidName, Active: true);

        var validator = new ActionItemTypeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IActionItemTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionItemType?)null);
        var model = new ActionItemTypeUpdateDto(Id: Guid.Empty, Name: TextData.ShortName, Active: true);

        var validator = new ActionItemTypeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
