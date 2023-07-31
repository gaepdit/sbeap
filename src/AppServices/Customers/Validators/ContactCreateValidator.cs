using FluentValidation;
using Sbeap.AppServices.Customers.Dto;

namespace Sbeap.AppServices.Customers.Validators;

public class ContactCreateValidator : AbstractValidator<ContactCreateDto>
{
    public ContactCreateValidator()
    {
        RuleFor(e => e.Email).EmailAddress();

        RuleFor(e => e.Title)
            .Must((c, _) =>
                !string.IsNullOrWhiteSpace(c.GivenName) ||
                !string.IsNullOrWhiteSpace(c.FamilyName) ||
                !string.IsNullOrWhiteSpace(c.Title))
            .WithMessage("At least a name or title must be entered to create a contact.");

        // Embedded phone number
        RuleFor(e => e.PhoneNumber)
            .SetValidator(new PhoneNumberCreateValidator())
            .When(e => !e.PhoneNumber.IsIncomplete);
    }
}
