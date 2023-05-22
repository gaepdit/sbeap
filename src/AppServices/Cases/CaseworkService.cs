using AutoMapper;
using GaEpd.AppLibrary.Pagination;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Staff.Dto;
using Sbeap.AppServices.UserServices;
using Sbeap.Domain.Entities.ActionItems;
using Sbeap.Domain.Entities.Agencies;
using Sbeap.Domain.Entities.Cases;
using Sbeap.Domain.Entities.Customers;

namespace Sbeap.AppServices.Cases;

public sealed class CaseworkService : ICaseworkService
{
    private readonly IMapper _mapper;
    private readonly IUserService _users;
    private readonly ICaseworkRepository _cases;
    private readonly ICaseworkManager _manager;
    private readonly IActionItemRepository _actionItems;
    private readonly ICustomerRepository _customers;
    private readonly IAgencyRepository _agencies;

    public CaseworkService(
        IMapper mapper, IUserService users, ICaseworkRepository cases, ICaseworkManager manager,
        IActionItemRepository actionItems, ICustomerRepository customers, IAgencyRepository agencies)
    {
        _mapper = mapper;
        _users = users;
        _cases = cases;
        _manager = manager;
        _actionItems = actionItems;
        _customers = customers;
        _agencies = agencies;
    }

    // Casework read

    public async Task<IPaginatedResult<CaseworkSearchResultDto>> SearchAsync(
        CaseworkSearchDto spec, PaginatedRequest paging, CancellationToken token = default)
    {
        var predicate = CaseworkFilters.CaseworkSearchPredicate(spec);

        var count = await _cases.CountAsync(predicate, token);

        var list = count > 0
            ? _mapper.Map<List<CaseworkSearchResultDto>>(await _cases.GetPagedListAsync(predicate, paging, token))
            : new List<CaseworkSearchResultDto>();

        return new PaginatedResult<CaseworkSearchResultDto>(list, count, paging);
    }

    public async Task<CaseworkViewDto?> FindAsync(Guid id, CancellationToken token = default)
    {
        var casework = await _cases.FindIncludeAllAsync(id, token);
        if (casework is null) return null;

        var caseworkView = _mapper.Map<CaseworkViewDto>(casework);
        if (casework.DeletedById != null)
            caseworkView.DeletedBy = _mapper.Map<StaffViewDto>(await _users.FindUserAsync(casework.DeletedById));

        return caseworkView;
    }

    // Casework write

    public async Task<Guid> CreateAsync(CaseworkCreateDto resource, CancellationToken token = default)
    {
        var customer = await _customers.GetAsync(resource.CustomerId, token);
        var item = _manager.Create(customer, resource.CaseOpenedDate, (await _users.GetCurrentUserAsync())?.Id);

        item.Description = resource.Description ?? string.Empty;

        await _cases.InsertAsync(item, token: token);
        return item.Id;
    }

    public async Task<CaseworkUpdateDto?> FindForUpdateAsync(Guid id, CancellationToken token = default) =>
        _mapper.Map<CaseworkUpdateDto>(await _cases.FindAsync(id, token));

    public async Task UpdateAsync(CaseworkUpdateDto resource, CancellationToken token = default)
    {
        var item = await _cases.GetAsync(resource.Id, token);
        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);

        item.CaseOpenedDate = resource.CaseOpenedDate;
        item.Description = resource.Description;
        item.CaseClosedDate = resource.CaseClosedDate;
        item.CaseClosureNotes = resource.CaseClosureNotes;
        item.ReferralDate = resource.ReferralDate;
        item.ReferralNotes = resource.ReferralNotes;
        if (resource.ReferralAgencyId is not null)
            item.ReferralAgency = await _agencies.GetAsync(resource.ReferralAgencyId.Value, token);

        await _cases.UpdateAsync(item, token: token);
    }

    public async Task DeleteAsync(Guid id, string? deleteComments, CancellationToken token = default)
    {
        var item = await _cases.GetAsync(id, token);
        item.SetDeleted((await _users.GetCurrentUserAsync())?.Id);
        item.DeleteComments = deleteComments;
        await _cases.UpdateAsync(item, token: token);
    }

    public async Task RestoreAsync(Guid id, CancellationToken token = default)
    {
        var item = await _cases.GetAsync(id, token);
        item.SetNotDeleted();
        await _cases.UpdateAsync(item, token: token);
    }

    // Action Items

    public async Task AddActionItemAsync(ActionItemCreateDto resource, CancellationToken token = default)
    {
        var casework = await _cases.GetAsync(resource.CaseworkId, token);
        var item = _manager.CreateActionItem(casework, resource.ActionItemType,
            (await _users.GetCurrentUserAsync())?.Id);

        item.ActionDate = resource.ActionDate;
        item.Notes = resource.Notes;

        await _actionItems.InsertAsync(item, token: token);
    }

    public async Task<ActionItemUpdateDto?> FindActionItemForUpdateAsync(Guid id, CancellationToken token = default) =>
        _mapper.Map<ActionItemUpdateDto>(await _actionItems.FindAsync(id, token));


    public async Task UpdateActionItemAsync(ActionItemUpdateDto resource, CancellationToken token = default)
    {
        var item = await _actionItems.GetAsync(resource.Id, token);
        item.SetUpdater((await _users.GetCurrentUserAsync())?.Id);

        item.ActionItemType = resource.ActionItemType;
        item.ActionDate = resource.ActionDate;
        item.Notes = resource.Notes;

        await _actionItems.UpdateAsync(item, token: token);
    }

    public async Task DeleteActionItemAsync(Guid actionItemId, CancellationToken token = default)
    {
        var item = await _actionItems.GetAsync(actionItemId, token);
        item.SetDeleted((await _users.GetCurrentUserAsync())?.Id);
        await _actionItems.UpdateAsync(item, token: token);
    }

    public void Dispose()
    {
        _cases.Dispose();
        _actionItems.Dispose();
    }
}
