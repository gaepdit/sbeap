using GaEpd.AppLibrary.Domain.Predicates;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.Domain.Entities.Cases;
using System.Linq.Expressions;

namespace Sbeap.AppServices.Cases;

public static class CaseworkFilters
{
    public static Expression<Func<Casework, bool>> CaseworkSearchPredicate(CaseworkSearchDto spec) =>
        PredicateBuilder.True<Casework>()
            .ByStatus(spec.Status)
            .ByDeletedStatus(spec.DeletedStatus)
            .ContainsCustomerName(spec.CustomerName)
            .ContainsDescription(spec.Description)
            .FromOpenedDate(spec.OpenedFrom)
            .ToOpenedDate(spec.OpenedTo)
            .FromClosedDate(spec.ClosedTo)
            .ToClosedDate(spec.ClosedFrom);

    private static Expression<Func<Casework, bool>> ByStatus(this Expression<Func<Casework, bool>> predicate,
        CaseStatus? input) => input switch
    {
        CaseStatus.Closed => predicate.And(e => e.IsClosed),
        CaseStatus.Open => predicate.And(e => !e.IsClosed),
        _ => predicate,
    };

    private static Expression<Func<Casework, bool>> ByDeletedStatus(this Expression<Func<Casework, bool>> predicate,
        CaseDeletedStatus? input) => input switch
    {
        CaseDeletedStatus.All => predicate,
        CaseDeletedStatus.Deleted => predicate.And(e => e.IsDeleted),
        _ => predicate.And(e => !e.IsDeleted),
    };

    private static Expression<Func<Casework, bool>> ContainsCustomerName(
        this Expression<Func<Casework, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input)
        ? predicate
        : predicate.And(e => e.Customer.Name.Contains(input));

    private static Expression<Func<Casework, bool>> ContainsDescription(this Expression<Func<Casework, bool>> predicate,
        string? input) => string.IsNullOrWhiteSpace(input)
        ? predicate
        : predicate.And(e => e.Description.Contains(input));

    private static Expression<Func<Casework, bool>> FromOpenedDate(
        this Expression<Func<Casework, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.CaseOpenedDate >= input);

    private static Expression<Func<Casework, bool>> ToOpenedDate(
        this Expression<Func<Casework, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.CaseOpenedDate <= input);

    private static Expression<Func<Casework, bool>> FromClosedDate(
        this Expression<Func<Casework, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e =>
                e.IsClosed && e.CaseClosedDate != null &&
                e.CaseClosedDate >= input);

    private static Expression<Func<Casework, bool>> ToClosedDate(
        this Expression<Func<Casework, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e =>
                e.IsClosed && e.CaseClosedDate != null &&
                e.CaseClosedDate <= input);
}
