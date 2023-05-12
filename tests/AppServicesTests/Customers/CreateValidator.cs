using FluentValidation.TestHelper;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Validators;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Customers;

public class CreateValidator
{
    [Test]
    public async Task ValidDtoWithNullContact_ReturnsAsValid()
    {
        var model = new CustomerCreateDto { Name = TextData.ValidName };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task ValidDtoWithEmptyContact_ReturnsAsValid()
    {
        var model = new CustomerCreateDto
        {
            Name = TextData.ValidName,
            Contact = ContactCreateDto.EmptyContact,
        };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task ValidDtoWithValidContact_ReturnsAsValid()
    {
        var model = new CustomerCreateDto
        {
            Name = TextData.ValidName,
            Contact = new ContactCreateDto { Title = TextData.Phrase },
        };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var model = new CustomerCreateDto { Name = TextData.ShortName };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task ContactWithoutNameOrTitle_ReturnsAsInvalid()
    {
        var model = new CustomerCreateDto
        {
            Name = TextData.ValidName,
            Contact = new ContactCreateDto { Email = TextData.ValidEmail },
        };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Contact);
    }
}
