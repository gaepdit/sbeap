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

        // Embedded Contact
        RuleFor(e => e.Contact)
            .SetValidator(new ContactCreateValidator())
            .When(e => !e.Contact.IsEmpty());
    }
}
