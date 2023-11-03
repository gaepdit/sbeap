using FluentValidation.TestHelper;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Validators;
using Sbeap.Domain.Entities.SicCodes;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Customers;

public class UpdateValidator
{
    private static CustomerUpdateDto DefaultCustomerUpdate => new();

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var model = DefaultCustomerUpdate with { Name = TextData.ValidName };
        var validator = new CustomerUpdateValidator(Substitute.For<ISicRepository>());

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var model = DefaultCustomerUpdate with { Name = TextData.ShortName };
        var validator = new CustomerUpdateValidator(Substitute.For<ISicRepository>());

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task CustomerWithValidSic_ReturnsAsValid()
    {
        var model = DefaultCustomerUpdate with
        {
            Name = TextData.ValidName,
            SicCodeId = "0000",
        };
        var sic = Substitute.For<ISicRepository>();
        sic.ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);
        var validator = new CustomerUpdateValidator(sic);

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task CustomerWithInvalidSic_ReturnsAsInvalid()
    {
        var model = DefaultCustomerUpdate with
        {
            Name = TextData.ShortName,
            SicCodeId = "0000",
        };
        var sic = Substitute.For<ISicRepository>();
        sic.ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        var validator = new CustomerUpdateValidator(sic);

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
