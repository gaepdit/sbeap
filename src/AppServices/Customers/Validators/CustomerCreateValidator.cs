using FluentValidation;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.SicCodes;

namespace Sbeap.AppServices.Customers.Validators;

public class CustomerCreateValidator : AbstractValidator<CustomerCreateDto>
{
    public CustomerCreateValidator(ISicRepository sic, IValidator<ContactCreateDto> contactValidator)
    {
        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(Customer.MinNameLength);

        RuleFor(e => e.Website)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("The Website must be a valid web address.")
            .When(x => !string.IsNullOrEmpty(x.Website));

        RuleFor(e => e.SicCodeId)
            .MustAsync(async (id, token) => id is null || await sic.ExistsAsync(id, token))
            .WithMessage(_ => "The SIC Code entered does not exist.");

        // Embedded Contact
        RuleFor(e => e.Contact)
            .SetValidator(contactValidator)
            .When(e => !e.Contact.IsEmpty);
    }
}
