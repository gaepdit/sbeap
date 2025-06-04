using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Customers;

namespace Sbeap.AppServices.Cases;

public sealed class CaseworkService(
    IMapper mapper,
    IUserService users,
    ICaseworkRepository cases,
    ICaseworkManager manager,
    ICustomerRepository customers,
    IAgencyRepository agencies)
    : ICaseworkService
{
    // Casework read

    public async Task<IPaginatedResult<CaseworkSearchResultDto>> SearchAsync(
        CaseworkSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);

        var count = await cases.CountAsync(predicate, token);

        var list = count > 0
            ? mapper.Map<List<CaseworkSearchResultDto>>(await cases.GetPagedListAsync(predicate, paging, token))
            : new List<CaseworkSearchResultDto>();

        return new PaginatedResult<CaseworkSearchResultDto>(list, count, paging);
    }

    public async Task<CaseworkViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var casework = await cases.FindIncludeAllAsync(id, token);
        if (casework is null) return null;

        var view = mapper.Map<CaseworkViewDto>(casework);
        return casework is { IsDeleted: true, DeletedById: not null }
            ? view with { DeletedBy = mapper.Map<StaffViewDto>(await users.FindUserAsync(casework.DeletedById)) }
            : view;
    }

    public async Task<CaseworkSearchResultDto?> FindBasicInfoAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<CaseworkSearchResultDto>(await cases.FindAsync(id, token));

    // Casework write

    public async Task<Guid> CreateAsync(CaseworkCreateDto resource, CancellationToken token = default)
    {
        var customer = await customers.GetAsync(resource.CustomerId, token);
        var item = manager.Create(customer, resource.CaseOpenedDate, (await users.GetCurrentUserAsync())?.Id);

        item.Description = resource.Description ?? string.Empty;

        await cases.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<CaseworkUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        mapper.Map<CaseworkUpdateDto>(await cases.FindAsync(id, token));

    public async Task UpdateAsync(Guid id, CaseworkUpdateDto resource, CancellationToken token = default)
    {
        var item = await cases.GetAsync(id, token);
        item.SetUpdater((await users.GetCurrentUserAsync())?.Id);

        item.CaseOpenedDate = resource.CaseOpenedDate;
        item.Description = resource.Description;
        item.CaseClosedDate = resource.CaseClosedDate;
        item.CaseClosureNotes = resource.CaseClosureNotes;
        item.ReferralDate = resource.ReferralDate;
        item.ReferralNotes = resource.ReferralNotes;
        if (resource.ReferralAgencyId is not null)
            item.ReferralAgency = await agencies.GetAsync(resource.ReferralAgencyId.Value, token);

        await cases.UpdateAsync(item, token: token);
    }

    public async Task DeleteAsync(Guid id, string? deleteComments, CancellationToken token = default)
    {
        var item = await cases.GetAsync(id, token);
        item.SetDeleted((await users.GetCurrentUserAsync())?.Id);
        item.DeleteComments = deleteComments;
        await cases.UpdateAsync(item, token: token);
    }

    public async Task RestoreAsync(Guid id, CancellationToken token = default)
    {
        var item = await cases.GetAsync(id, token);
        item.SetNotDeleted();
        await cases.UpdateAsync(item, token: token);
    }

    public void Dispose()
    {
        cases.Dispose();
        customers.Dispose();
        agencies.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await cases.DisposeAsync();
        await customers.DisposeAsync();
        await agencies.DisposeAsync();
    }
}
