using FluentValidation;
using FluentValidation.TestHelper;
using Sbeap.AppServices.Offices;
using Sbeap.AppServices.Offices.Validators;
using Sbeap.Domain.Entities.Offices;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Offices.Validators;

public class UpdateValidator
{
    private static ValidationContext<OfficeUpdateDto> GetContext(OfficeUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((Office?)null);
        var model = new OfficeUpdateDto(Name: TextData.ValidName, Active: true);

        var result = await new OfficeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Office(Guid.NewGuid(), TextData.ValidName));
        var model = new OfficeUpdateDto(Name: TextData.ValidName, Active: true);

        var result = await new OfficeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Office(Guid.Empty, TextData.ValidName));
        var model = new OfficeUpdateDto(Name: TextData.ValidName, Active: true);

        var result = await new OfficeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((Office?)null);
        var model = new OfficeUpdateDto(Name: TextData.ShortName, Active: true);

        var result = await new OfficeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
