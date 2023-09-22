using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.Domain.Data;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Customers;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class EditModel : PageModel
{
    // Constructor
    private readonly ICustomerService _service;
    private readonly IValidator<CustomerUpdateDto> _validator;
    private readonly IAuthorizationService _authorization;

    public EditModel(ICustomerService service, IValidator<CustomerUpdateDto> validator,
        IAuthorizationService authorization)
    {
        _service = service;
        _validator = validator;
        _authorization = authorization;
    }

    // Properties
    [BindProperty]
    public CustomerUpdateDto Item { get; set; } = default!;

    // Select lists
    public SelectList StatesSelectList => new(Data.States);
    public SelectList CountiesSelectList => new(Data.Counties);

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await _service.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();
        if (!await UserCanEditAsync(item)) return Forbid();

        Item = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var original = await _service.FindForUpdateAsync(Item.Id);
        if (original is null) return BadRequest();
        if (!await UserCanEditAsync(original)) return BadRequest();

        await _validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        await _service.UpdateAsync(Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Customer successfully updated.");
        return RedirectToPage("Details", new { Item.Id });
    }

    private async Task<bool> UserCanEditAsync(CustomerUpdateDto item) =>
        (await _authorization.AuthorizeAsync(User, item, CustomerOperation.Edit)).Succeeded;
}
