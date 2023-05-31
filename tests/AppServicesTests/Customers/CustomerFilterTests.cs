using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.TestData;

namespace AppServicesTests.Customers;

public class CustomerFilterTests
{
    private static CustomerSearchDto DefaultCustomerSearchDto => new();

    [Test]
    public void DefaultFilter_ReturnsAllNotDeleted()
    {
        // Arrange
        var spec = DefaultCustomerSearchDto;
        var expected = CustomerData.GetCustomers.Where(e => !e.IsDeleted);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeletedSpec_ReturnsAllDeleted()
    {
        // Arrange
        var spec = DefaultCustomerSearchDto with { DeletedStatus = CustomerDeletedStatus.Deleted };
        var expected = CustomerData.GetCustomers.Where(e => e.IsDeleted);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeletedSpecNeutral_ReturnsAll()
    {
        // Arrange
        var spec = DefaultCustomerSearchDto with { DeletedStatus = CustomerDeletedStatus.All };

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(CustomerData.GetCustomers);
    }

    [Test]
    public void TextNameSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CustomerData.GetCustomers.First(e => !e.IsDeleted);
        var spec = DefaultCustomerSearchDto with { Name = referenceItem.Name };
        var expected = CustomerData.GetCustomers.Where(e => !e.IsDeleted && e.Name == referenceItem.Name);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TextCountySpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CustomerData.GetCustomers.First(e => !e.IsDeleted && !string.IsNullOrEmpty(e.County));
        var spec = DefaultCustomerSearchDto with { County = referenceItem.County };
        var expected = CustomerData.GetCustomers.Where(e => !e.IsDeleted && e.County == referenceItem.County);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void TextDescriptionSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = CustomerData.GetCustomers.First(e => !e.IsDeleted && !string.IsNullOrEmpty(e.Description));
        var spec = DefaultCustomerSearchDto with { Description = referenceItem.Description };
        var expected = CustomerData.GetCustomers.Where(e => !e.IsDeleted && e.Description == referenceItem.Description);

        // Act
        var predicate = CustomerFilters.CustomerSearchPredicate(spec);
        var result = CustomerData.GetCustomers
            .Where(predicate.Compile()).AsQueryable().ToList();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
