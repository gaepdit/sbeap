using FluentValidation;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Entities.Customers;
using Sbeap.Domain.Entities.SicCodes;

namespace Sbeap.AppServices.Customers.Validators;

public class CustomerUpdateValidator : AbstractValidator<CustomerUpdateDto>
{
    public CustomerUpdateValidator(ISicRepository sic)
    {
        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(Customer.MinNameLength);

        RuleFor(e => e.SicCodeId)
            .MustAsync(async (id, token) => id is null || await sic.ExistsAsync(id, token))
            .WithMessage(e => $"The SIC Code entered does not exist.");
    }
}
