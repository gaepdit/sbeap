using FluentValidation;
using FluentValidation.TestHelper;
using Sbeap.AppServices.ActionItemTypes;
using Sbeap.AppServices.ActionItemTypes.Validators;
using Sbeap.Domain.Entities.ActionItemTypes;
using Sbeap.TestData.Constants;

namespace AppServicesTests.ActionItemTypes;

internal class UpdateValidator
{
    private static ValidationContext<ActionItemTypeUpdateDto> GetContext(ActionItemTypeUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var model = new ActionItemTypeUpdateDto(Name: TextData.ValidName, Active: true);

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionItemType(Guid.NewGuid(), TextData.ValidName));
        var model = new ActionItemTypeUpdateDto(Name: TextData.ValidName, Active: true);

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ActionItemType(Guid.Empty, TextData.ValidName));
        var model = new ActionItemTypeUpdateDto(Name: TextData.ValidName, Active: true);

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var model = new ActionItemTypeUpdateDto(Name: TextData.ShortName, Active: true);

        var result = await new ActionItemTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
