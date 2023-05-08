using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.TestData;

namespace AppServicesTests.Cases;

public class CaseworkFilterTests
{
    [Test]
    public void DefaultFilter_ReturnsAllNotDeleted()
    {
        // Arrange
        var spec = new CaseworkSearchDto();
        var expected = CaseworkData.GetCases.Where(e => !e.IsDeleted);

        // Act
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);
        var result = CaseworkData.GetCases
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeletedSpec_ReturnsAllDeleted()
    {
        // Arrange
        var spec = new CaseworkSearchDto { DeletedStatus = CaseDeletedStatus.Deleted };
        var expected = CaseworkData.GetCases.Where(e => e.IsDeleted);

        // Act
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);
        var result = CaseworkData.GetCases
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeletedSpecNeutral_ReturnsAll()
    {
        // Arrange
        var spec = new CaseworkSearchDto { DeletedStatus = CaseDeletedStatus.All };

        // Act
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);
        var result = CaseworkData.GetCases
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(CaseworkData.GetCases);
    }

    [Test]
    public void StatusOpen_ReturnsFilteredList()
    {
        // Arrange
        var spec = new CaseworkSearchDto { Status = CaseStatus.Open };
        var expected = CaseworkData.GetCases.Where(e => e is { IsDeleted: false, IsClosed: false });

        // Act
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);
        var result = CaseworkData.GetCases
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void StatusClosed_ReturnsFilteredList()
    {
        // Arrange
        var spec = new CaseworkSearchDto { Status = CaseStatus.Closed };
        var expected = CaseworkData.GetCases.Where(e => e is { IsDeleted: false, IsClosed: true });

        // Act
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);
        var result = CaseworkData.GetCases
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TextCustomerNameSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CaseworkData.GetCases.First(e => !e.IsDeleted && !string.IsNullOrEmpty(e.Customer.Name));
        var spec = new CaseworkSearchDto { CustomerName = referenceItem.Customer.Name };
        var expected = CaseworkData.GetCases.Where(e => !e.IsDeleted && e.Customer.Name == referenceItem.Customer.Name);

        // Act
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);
        var result = CaseworkData.GetCases
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TextDescriptionSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CaseworkData.GetCases.First(e => !e.IsDeleted && !string.IsNullOrEmpty(e.Description));
        var spec = new CaseworkSearchDto { Description = referenceItem.Description };
        var expected = CaseworkData.GetCases.Where(e => !e.IsDeleted && e.Description == referenceItem.Description);

        // Act
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);
        var result = CaseworkData.GetCases
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DateOpenedSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CaseworkData.GetCases.First(e => !e.IsDeleted);
        var spec = new CaseworkSearchDto
            { OpenedFrom = referenceItem.CaseOpenedDate, OpenedTo = referenceItem.CaseOpenedDate };

        var expected = CaseworkData.GetCases
            .Where(e => !e.IsDeleted && e.CaseOpenedDate == referenceItem.CaseOpenedDate);

        // Act
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);
        var result = CaseworkData.GetCases
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DateClosedSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CaseworkData.GetCases.First(e => e is { IsDeleted: false, IsClosed: true });
        var spec = new CaseworkSearchDto
            { ClosedFrom = referenceItem.CaseClosedDate, ClosedTo = referenceItem.CaseClosedDate };

        var expected = CaseworkData.GetCases
            .Where(e => !e.IsDeleted && e.CaseClosedDate == referenceItem.CaseClosedDate);

        // Act
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);
        var result = CaseworkData.GetCases
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
