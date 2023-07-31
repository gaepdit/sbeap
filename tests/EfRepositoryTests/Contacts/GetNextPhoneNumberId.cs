using Sbeap.Domain.Entities.Contacts;

namespace EfRepositoryTests.Contacts;

public class GetNextPhoneNumberId
{
    private IContactRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = RepositoryHelper.CreateRepositoryHelper().GetContactRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    [Test]
    public void GivenEF_ReturnsZero()
    {
        _repository.GetNextPhoneNumberId().Should().Be(0);
    }
}
