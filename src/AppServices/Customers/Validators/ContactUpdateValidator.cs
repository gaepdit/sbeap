using FluentValidation;
using Sbeap.AppServices.Customers.Dto;

namespace Sbeap.AppServices.Customers.Validators;

[UsedImplicitly]
public class ContactUpdateValidator : AbstractValidator<ContactUpdateDto>
{
    public ContactUpdateValidator()
    {
        RuleFor(e => e.Email).EmailAddress();

        RuleFor(e => e)
            .Must(c =>
                !string.IsNullOrWhiteSpace(c.GivenName) ||
                !string.IsNullOrWhiteSpace(c.FamilyName) ||
                !string.IsNullOrWhiteSpace(c.Title))
            .OverridePropertyName(c => c.Title)
            .WithMessage("At least a name or title must be entered to create a contact.");
    }
}
