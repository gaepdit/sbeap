using NUnit.Framework;
using Sbeap.Domain.Identity;

namespace Sbeap.WebAppTests.Pages.Account
{
    public class UserDomainValidationTests
    {
        [Test]
        public void IsValidEmailDomain_ValidEmail_ReturnsTrue()
        {
            var email = "test@dnr.ga.gov";

            var result = email.IsValidEmailDomain();

            result.Should().BeTrue();
        }

        [Test]
        public void IsValidEmailDomain_InvalidEmail_ReturnsFalse()
        {
            var email = "test@invalid.com";

            var result = email.IsValidEmailDomain();

            result.Should().BeFalse();
        }
    }
}