using FluentValidation;
using Sbeap.AppServices.Customers.Dto;

namespace Sbeap.AppServices.Customers.Validators;

public class PhoneNumberCreateValidator : AbstractValidator<PhoneNumberCreate>
{
    public PhoneNumberCreateValidator()
    {
        RuleFor(e => e)
            .Must(c => !string.IsNullOrWhiteSpace(c.Number))
            .WithMessage("Phone number must be entered.")
            .Must(c => c.Type != null)
            .WithMessage("Phone number type must be selected.");
    }
}
