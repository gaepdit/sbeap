using Sbeap.LocalRepository.Repositories;

namespace LocalRepositoryTests.Contacts;

public class GetNextPhoneNumberId
{
    private LocalContactRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalContactRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();


    [Test]
    public void GivenLocalRepository_ReturnsNextIdNumber()
    {
        var maxId = _repository.Items
            .SelectMany(e => e.PhoneNumbers)
            .Max(e => e.Id);
        _repository.GetNextPhoneNumberId().Should().Be(maxId + 1);
    }
}
