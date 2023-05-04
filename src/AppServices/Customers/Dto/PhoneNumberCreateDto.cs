using Sbeap.Domain.ValueObjects;

namespace Sbeap.AppServices.Customers.Dto;

public class PhoneNumberCreateDto
{
    public PhoneNumber PhoneNumber { get; init; } = default!;
}
