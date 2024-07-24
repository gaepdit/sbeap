using Sbeap.Domain.Identity;

namespace WebAppTests.Pages.Account;

public class UserDomainValidationTests
{
    [Test]
    public void IsValidEmailDomain_ValidEmail_ReturnsTrue()
    {
        const string email = "test@dnr.ga.gov";

        var result = email.IsValidEmailDomain();

        result.Should().BeTrue();
    }

    [Test]
    public void IsValidEmailDomain_InvalidEmail_ReturnsFalse()
    {
        const string email = "test@invalid.com";

        var result = email.IsValidEmailDomain();

        result.Should().BeFalse();
    }
}
