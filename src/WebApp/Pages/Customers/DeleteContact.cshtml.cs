using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Customers.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Customers;

public class DeleteContactModel(ICustomerService service, IAuthorizationService authorization)
    : PageModel
{
    // Properties
    [BindProperty]
    public Guid ContactId { get; set; }

    public ContactUpdateDto ContactView { get; private set; } = default!;
    public CustomerSearchResultDto CustomerView { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");

        var contact = await service.FindContactForUpdateAsync(id.Value);
        if (contact is null) return NotFound();
        if (!await UserCanDeleteContactsAsync(contact)) return Forbid();

        var customer = await service.FindBasicInfoAsync(contact.CustomerId);
        if (customer is null || customer.IsDeleted) return NotFound();

        ContactView = contact;
        CustomerView = customer;
        ContactId = id.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return BadRequest();

        var originalContact = await service.FindContactForUpdateAsync(ContactId);
        if (originalContact is null || !await UserCanDeleteContactsAsync(originalContact)) return BadRequest();

        var customer = await service.FindBasicInfoAsync(originalContact.CustomerId);
        if (customer is null || customer.IsDeleted) return BadRequest();

        await service.DeleteContactAsync(ContactId);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Contact successfully deleted.");
        return RedirectToPage("Details", new { customer.Id });
    }

    private async Task<bool> UserCanDeleteContactsAsync(ContactUpdateDto item) =>
        (await authorization.AuthorizeAsync(User, item, CustomerOperation.Edit)).Succeeded;
}
