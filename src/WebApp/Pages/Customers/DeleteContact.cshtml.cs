using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Customers;
using Sbeap.AppServices.Customers.Dto;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;

namespace Sbeap.WebApp.Pages.Customers;

[Authorize(Policy = nameof(Policies.AdminUser))]
public class DeleteContactModel : PageModel
{
    // Constructor
    private readonly ICustomerService _service;
    private readonly IAuthorizationService _authorization;

    public DeleteContactModel(ICustomerService service, IAuthorizationService authorization)
    {
        _service = service;
        _authorization = authorization;
    }

    // Properties
    [BindProperty]
    public Guid ContactId { get; set; }

    public ContactViewDto ContactView { get; private set; } = default!;
    public CustomerSearchResultDto CustomerView { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        if (!await UserCanManageDeletionsAsync()) return NotFound();

        var contact = await _service.FindContactAsync(id.Value);
        if (contact is null) return NotFound();

        var customer = await _service.FindBasicInfoAsync(contact.CustomerId);
        if (customer is null || customer.IsDeleted) return NotFound();

        ContactView = contact;
        CustomerView = customer;
        ContactId = id.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!await UserCanManageDeletionsAsync() || !ModelState.IsValid) return BadRequest();

        var originalContact = await _service.FindContactAsync(ContactId);
        if (originalContact is null) return BadRequest();

        var customer = await _service.FindBasicInfoAsync(originalContact.CustomerId);
        if (customer is null || customer.IsDeleted) return BadRequest();

        await _service.DeleteContactAsync(ContactId);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Contact successfully deleted.");
        return RedirectToPage("Details", new { customer.Id });
    }

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await _authorization.AuthorizeAsync(User, nameof(Policies.AdminUser))).Succeeded;
}
