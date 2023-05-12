using FluentValidation;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Entities.Customers;

namespace Sbeap.AppServices.Customers.Validators;

public class CustomerCreateValidator : AbstractValidator<CustomerCreateDto>
{
    public CustomerCreateValidator()
    {
        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(Customer.MinNameLength);

        RuleFor(e => e.Website)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.Website));

        RuleFor(e => e.Contact)
            .Must(e =>
                e is null ||
                e.Equals(ContactCreateDto.EmptyContact) ||
                !string.IsNullOrWhiteSpace(e.GivenName) ||
                !string.IsNullOrWhiteSpace(e.FamilyName) ||
                !string.IsNullOrWhiteSpace(e.Title))
            .WithMessage("At least a name or title must be entered to create a contact.");
    }
}
