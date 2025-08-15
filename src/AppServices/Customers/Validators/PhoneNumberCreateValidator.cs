using FluentValidation;
using Sbeap.AppServices.Customers.Dto;

namespace Sbeap.AppServices.Customers.Validators;

public class PhoneNumberCreateValidator : AbstractValidator<PhoneNumberCreate>
{
    public PhoneNumberCreateValidator()
    {
        RuleFor(e => e.Number)
            .NotEmpty()
            .WithMessage("Phone number must be entered.");

        RuleFor(e => e.Type)
            .NotNull()
            .WithMessage("Phone number type must be selected.");
    }
}
