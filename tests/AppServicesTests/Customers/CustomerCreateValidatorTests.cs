using FluentValidation.TestHelper;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Validators;
using Sbeap.TestData.Constants;

namespace AppServicesTests.Customers;

public class CustomerCreateValidatorTests
{
    private static ContactCreateDto EmptyContactCreate => new();

    private static CustomerCreateDto EmptyCustomerCreate => new();

    [Test]
    public async Task ValidDtoWithEmptyContact_ReturnsAsValid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = TextData.ValidName,
            Contact = EmptyContactCreate,
        };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task ValidDtoWithValidContact_ReturnsAsValid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = TextData.ValidName,
            Contact = EmptyContactCreate with { Title = TextData.Phrase },
        };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = TextData.ShortName,
            Contact = EmptyContactCreate,
        };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task InvalidWebsite_ReturnsAsInvalid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = TextData.ValidName,
            Website = TextData.NonExistentName, // invalid as website
            Contact = EmptyContactCreate,
        };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Website);
    }

    [Test]
    public async Task InvalidContactEmail_ReturnsAsInvalid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = TextData.ValidName,
            Contact = EmptyContactCreate with
            {
                Title = TextData.Phrase,
                Email = TextData.NonExistentName, // invalid as email
            },
        };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Contact.Email);
    }

    [Test]
    public async Task ContactWithoutNameOrTitle_ReturnsAsInvalid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = TextData.ValidName,
            Contact = EmptyContactCreate with { Email = TextData.ValidEmail },
        };
        var validator = new CustomerCreateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Contact.Title);
    }
}
