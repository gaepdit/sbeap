using GaEpd.AppLibrary.Domain.Predicates;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.Domain.Entities.Cases;
using System.Linq.Expressions;

namespace Sbeap.AppServices.Cases;

public static class CaseworkFilters
{
    public static Expression<Func<Casework, bool>> CaseworkSearchPredicate(CaseworkSearchDto spec) =>
        PredicateBuilder.True<Casework>()
            .ByStatus(spec.Status)
            .ByCaseDeletedStatus(spec.DeletedStatus)
            .ContainsDescription(spec.Description)
            .ContainsCustomerName(spec.CustomerName)
            .ByCustomerDeletedStatus(spec.CustomerDeletedStatus)
            .FromOpenedDate(spec.OpenedFrom)
            .ThroughOpenedDate(spec.OpenedThrough)
            .FromClosedDate(spec.ClosedFrom)
            .ThroughClosedDate(spec.ClosedThrough)
            .ReferredTo(spec.ReferralAgency)
            .FromReferralDate(spec.ReferredFrom)
            .ThroughReferralDate(spec.ReferredThrough);

    private static Expression<Func<Casework, bool>> ByStatus(this Expression<Func<Casework, bool>> predicate,
        CaseStatus? input) => input switch
    {
        CaseStatus.Closed => predicate.And(e => e.CaseClosedDate != null),
        CaseStatus.Open => predicate.And(e => e.CaseClosedDate == null),
        _ => predicate,
    };

    private static Expression<Func<Casework, bool>> ByCaseDeletedStatus(this Expression<Func<Casework, bool>> predicate,
        CaseDeletedStatus? input) => input switch
    {
        CaseDeletedStatus.All => predicate,
        CaseDeletedStatus.Deleted => predicate.And(e => e.IsDeleted),
        _ => predicate.And(e => !e.IsDeleted),
    };

    private static Expression<Func<Casework, bool>> ByCustomerDeletedStatus(
        this Expression<Func<Casework, bool>> predicate,
        CustomerDeletedStatus? input) => input switch
    {
        CustomerDeletedStatus.All => predicate,
        CustomerDeletedStatus.Deleted => predicate.And(e => e.Customer.IsDeleted),
        _ => predicate.And(e => !e.Customer.IsDeleted),
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

    private static Expression<Func<Casework, bool>> ThroughOpenedDate(
        this Expression<Func<Casework, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.CaseOpenedDate <= input);

    private static Expression<Func<Casework, bool>> FromClosedDate(
        this Expression<Func<Casework, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.CaseClosedDate != null && e.CaseClosedDate >= input);

    private static Expression<Func<Casework, bool>> ThroughClosedDate(
        this Expression<Func<Casework, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.CaseClosedDate != null && e.CaseClosedDate <= input);

    private static Expression<Func<Casework, bool>> ReferredTo(this Expression<Func<Casework, bool>> predicate,
        Guid? input) => input is null
        ? predicate
        : predicate.And(e => e.ReferralAgency != null && e.ReferralAgency.Id == input);

    private static Expression<Func<Casework, bool>> FromReferralDate(
        this Expression<Func<Casework, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.ReferralDate != null && e.ReferralDate >= input);

    private static Expression<Func<Casework, bool>> ThroughReferralDate(
        this Expression<Func<Casework, bool>> predicate, DateOnly? input) =>
        input is null
            ? predicate
            : predicate.And(e => e.ReferralDate != null && e.ReferralDate <= input);
}
