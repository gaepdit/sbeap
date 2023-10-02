using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.Agencies;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Cases;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class EditModel : PageModel
{
    private readonly ICaseworkService _service;
    private readonly IAgencyService _agencyService;
    private readonly IValidator<CaseworkUpdateDto> _validator;
    private readonly IAuthorizationService _authorization;

    public EditModel(ICaseworkService service, IAgencyService agencyService, IValidator<CaseworkUpdateDto> validator,
        IAuthorizationService authorization)
    {
        _service = service;
        _agencyService = agencyService;
        _validator = validator;
        _authorization = authorization;
    }

    // Properties

    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public CaseworkUpdateDto Item { get; set; } = default!;

    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public SelectList AgencySelectList { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await _service.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();

        await SetPermissionsAsync(item);

        if (UserCan[CaseworkOperation.Edit])
        {
            Id = id.Value;
            Item = item;
            await PopulateSelectListsAsync();
            return Page();
        }

        if (!UserCan[CaseworkOperation.ManageDeletions]) return NotFound();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted case.");
        return RedirectToPage("Details", new { id });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var originalItem = await _service.FindForUpdateAsync(Id);
        if (originalItem is null) return BadRequest();
        await SetPermissionsAsync(originalItem);
        if (!UserCan[CaseworkOperation.Edit]) return BadRequest();

        await _validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        await _service.UpdateAsync(Id, Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Case successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task PopulateSelectListsAsync() =>
        AgencySelectList = (await _agencyService.GetListItemsAsync()).ToSelectList();

    private async Task SetPermissionsAsync(CaseworkUpdateDto item)
    {
        foreach (var operation in CaseworkOperation.AllOperations)
            UserCan[operation] = (await _authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
