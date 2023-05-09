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
    }
}
