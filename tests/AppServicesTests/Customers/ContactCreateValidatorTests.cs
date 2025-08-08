using FluentValidation.TestHelper;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Validators;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Customers;

public class ContactCreateValidatorTests
{
    private static ContactCreateDto EmptyContactCreateDto => new(Guid.Empty);
    private static readonly PhoneNumberCreateValidator PhoneNumberValidator = new();
    private static readonly ContactCreateValidator ContactValidator = new(PhoneNumberValidator);

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var model = EmptyContactCreateDto with
        {
            GivenName = TextData.ValidName,
            Email = TextData.ValidEmail,
        };

        var result = await ContactValidator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task EmptyContact_ReturnsAsInvalid()
    {
        var result = await ContactValidator.TestValidateAsync(EmptyContactCreateDto);

        result.ShouldHaveValidationErrorFor(e => e.Title);
    }

    [Test]
    public async Task InvalidEmail_ReturnsAsInvalid()
    {
        var model = EmptyContactCreateDto with
        {
            Title = TextData.Phrase,
            Email = TextData.NonExistentName, // invalid as email
        };

        var result = await ContactValidator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Email);
    }

    [Test]
    public async Task MissingNameOrTitle_ReturnsAsInvalid()
    {
        var model = EmptyContactCreateDto with
        {
            Email = TextData.ValidEmail,
        };

        var result = await ContactValidator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Title);
    }
}
