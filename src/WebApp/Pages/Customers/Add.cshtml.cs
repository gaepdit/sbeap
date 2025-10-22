using FluentValidation;
using GaEpd.AppLibrary.ListItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.AuthorizationPolicies;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.SicCodes;
using Sbeap.Domain.Data;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Customers;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class AddModel(ICustomerService service, ISicService sicService, IValidator<CustomerCreateDto> validator)
    : PageModel
{
    // Properties
    [BindProperty]
    public CustomerCreateDto Item { get; set; } = default!;

    // Select lists
    public static SelectList StatesSelectList => new(StateData.States);
    public static SelectList CountiesSelectList => new(CountyData.Counties);
    public SelectList SicSelectList { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync()
    {
        await PopulateSelectListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync();
            return Page();
        }

        var id = await service.CreateAsync(Item);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "New Customer successfully added.");
        return RedirectToPage("Details", new { id });
    }

    private async Task PopulateSelectListsAsync() =>
        SicSelectList = (await sicService.GetActiveListItemsAsync()).ToSelectList();
}
