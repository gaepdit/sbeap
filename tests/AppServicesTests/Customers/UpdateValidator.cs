using FluentValidation.TestHelper;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Validators;
using Sbeap.Domain.ValueObjects;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Customers;

public class UpdateValidator
{
    private static CustomerUpdateDto DefaultCustomerUpdate => new(Guid.Empty, string.Empty, string.Empty, null,
        new IncompleteAddress(), new IncompleteAddress());

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var model = DefaultCustomerUpdate with { Name = TextData.ValidName };
        var validator = new CustomerUpdateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var model = DefaultCustomerUpdate with { Name = TextData.ShortName };
        var validator = new CustomerUpdateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
