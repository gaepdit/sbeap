namespace Sbeap.Domain.Identity;

public static class UserDomainValidation
{
    public static bool IsValidEmailDomain(this string email) =>
        email.EndsWith("@dnr.ga.gov", StringComparison.CurrentCultureIgnoreCase);
}
