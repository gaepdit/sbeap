using FluentValidation;
using FluentValidation.TestHelper;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Agencies.Validators;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Agencies;

internal class UpdateValidator
{
    private static ValidationContext<AgencyUpdateDto> GetContext(AgencyUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((Agency?)null);
        var model = new AgencyUpdateDto(Name: TextData.ValidName, Active: true);

        var result = await new AgencyUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Agency(Guid.NewGuid(), TextData.ValidName));
        var model = new AgencyUpdateDto(Name: TextData.ValidName, Active: true);

        var result = await new AgencyUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new Agency(Guid.Empty, TextData.ValidName));
        var model = new AgencyUpdateDto(Name: TextData.ValidName, Active: true);

        var result = await new AgencyUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((Agency?)null);
        var model = new AgencyUpdateDto(Name: TextData.ShortName, Active: true);

        var result = await new AgencyUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
