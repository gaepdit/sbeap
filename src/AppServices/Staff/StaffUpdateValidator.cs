using FluentValidation;
using Sbeap.Domain.Identity;

namespace Sbeap.AppServices.Staff;

public class StaffUpdateValidator : AbstractValidator<StaffUpdateDto>
{
    public StaffUpdateValidator()
    {
        RuleFor(e => e.Phone)
            .MaximumLength(ApplicationUser.MaxPhoneLength);
    }
}
