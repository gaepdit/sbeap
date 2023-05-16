using FluentValidation.TestHelper;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Validators;
using Sbeap.Domain.ValueObjects;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Customers;

public class ContactCreateValidatorTests
{
    private static ContactCreateDto EmptyContactCreateDto => new(null, null, null, null, null, null,
        new IncompleteAddress(), new PhoneNumber());

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var model = EmptyContactCreateDto with
        {
            GivenName = TextData.ValidName,
            Email = TextData.ValidEmail,
        };
        var validator = new ContactCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task EmptyContact_ReturnsAsInvalid()
    {
        var validator = new ContactCreateValidator();

        var result = await validator.TestValidateAsync(EmptyContactCreateDto);

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
        var validator = new ContactCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Email);
    }

    [Test]
    public async Task MissingNameOrTitle_ReturnsAsInvalid()
    {
        var model = EmptyContactCreateDto with
        {
            Email = TextData.ValidEmail,
        };
        var validator = new ContactCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Title);
    }
}
