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

    public DeleteContactModel(
        ICustomerService service,
        IAuthorizationService authorization)
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
    public async Task<IActionResult> OnGetAsync(Guid? id, Guid? contactId)
    {
        if (id is null || contactId is null) return RedirectToPage("Index");

        var customer = await _service.FindBasicInfoAsync(id.Value);
        if (customer is null) return NotFound();
        CustomerView = customer;

        if (CustomerView.IsDeleted || !await UserCanManageDeletionsAsync())
            return NotFound();

        var contact = await _service.FindContactAsync(contactId.Value);
        if (contact is null) return NotFound();
        ContactView = contact;

        ContactId = contactId.Value;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");

        var customer = await _service.FindBasicInfoAsync(id.Value);
        if (customer is null) return BadRequest();
        CustomerView = customer;

        if (CustomerView.IsDeleted || !await UserCanManageDeletionsAsync() || !ModelState.IsValid)
            return BadRequest();

        await _service.DeleteContactAsync(ContactId);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Contact successfully deleted.");
        return RedirectToPage("Details", new { id });
    }

    private async Task<bool> UserCanManageDeletionsAsync() =>
        (await _authorization.AuthorizeAsync(User, nameof(Policies.AdminUser))).Succeeded;
}
