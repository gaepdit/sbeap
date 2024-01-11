using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Permissions;
using Sbeap.Domain.Data;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Customers;

[Authorize(Policy = nameof(Policies.StaffUser))]
public class AddContactModel(
    ICustomerService service,
    IValidator<ContactCreateDto> validator,
    IAuthorizationService authorization)
    : PageModel
{
    // Properties
    [BindProperty]
    public ContactCreateDto NewContact { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CustomerSearchResultDto Customer { get; private set; } = default!;

    // Select lists
    public SelectList StatesSelectList => new(StateData.States);

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        var customer = await service.FindBasicInfoAsync(id.Value);
        if (customer is null) return NotFound();

        Customer = customer;
        NewContact = new ContactCreateDto(id.Value);

        if (!Customer.IsDeleted) return Page();
        if (!await UserCanManageDeletionsAsync()) return NotFound();
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot add a contact to a deleted customer.");
        return RedirectToPage("Details", new { id });
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");
        if (NewContact.CustomerId != id) return BadRequest();

        var customer = await service.FindBasicInfoAsync(id.Value);
        if (customer is null) return BadRequest();

        Customer = customer;
        if (Customer.IsDeleted) return Forbid();

        await validator.ApplyValidationAsync(NewContact, ModelState);
        if (!ModelState.IsValid) return Page();

        HighlightId = await service.AddContactAsync(NewContact);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "New Contact successfully added.");
        return RedirectToPage("Details", null, new { id }, "contacts");
    }

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await authorization.AuthorizeAsync(User, nameof(Policies.AdminUser))).Succeeded;
}
