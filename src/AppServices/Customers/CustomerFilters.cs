using GaEpd.AppLibrary.Domain.Predicates;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Entities.Customers;
using System.Linq.Expressions;

namespace Sbeap.AppServices.Customers;

public static class CustomerFilters
{
    public static Expression<Func<Customer, bool>> CustomerSearchPredicate(CustomerSearchDto spec) =>
        PredicateBuilder.True<Customer>()
            .ContainsName(spec.Name)
            .ContainsDescription(spec.Description)
            .HasSicCode(spec.SicCode)
            .InCity(spec.City)
            .InCounty(spec.County)
            .ByDeletedStatus(spec.DeletedStatus);

    private static Expression<Func<Customer, bool>> ContainsName(this Expression<Func<Customer, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input)
        ? predicate
        : predicate.And(e => e.Name.Contains(input));

    private static Expression<Func<Customer, bool>> ContainsDescription(this Expression<Func<Customer, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input)
        ? predicate
        : predicate.And(e => e.Description != null && e.Description.Contains(input));

    private static Expression<Func<Customer, bool>> HasSicCode(this Expression<Func<Customer, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input)
        ? predicate
        : predicate.And(e => e.SicCode != null && e.SicCode.Id == input);

    private static Expression<Func<Customer, bool>> InCity(this Expression<Func<Customer, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input)
        ? predicate
        : predicate.And(e =>
            (e.Location.City != null && e.Location.City.Contains(input)) ||
            (e.MailingAddress.City != null && e.MailingAddress.City.Contains(input)));

    private static Expression<Func<Customer, bool>> InCounty(this Expression<Func<Customer, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input) ? predicate : predicate.And(e => e.County == input);

    private static Expression<Func<Customer, bool>> ByDeletedStatus(this Expression<Func<Customer, bool>> predicate,
        CustomerDeletedStatus? input) => input switch
    {
        CustomerDeletedStatus.All => predicate,
        CustomerDeletedStatus.Deleted => predicate.And(e => e.IsDeleted),
        _ => predicate.And(e => !e.IsDeleted),
    };
}
