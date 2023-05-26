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

[Authorize(Policy = PolicyName.StaffUser)]
public class EditContactModel : PageModel
{
    // Constructor
    private readonly ICustomerService _service;
    private readonly IValidator<ContactUpdateDto> _validator;
    private readonly IAuthorizationService _authorization;

    public EditContactModel(
        ICustomerService service,
        IValidator<ContactUpdateDto> validator,
        IAuthorizationService authorization)
    {
        _service = service;
        _validator = validator;
        _authorization = authorization;
    }

    // Properties
    [BindProperty]
    public ContactUpdateDto UpdateContact { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CustomerSearchResultDto Customer { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public SelectList StatesSelectList => new(Data.States);

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id, Guid? contactId)
    {
        if (id is null || contactId is null) return RedirectToPage("../Index");

        var customer = await _service.FindBasicInfoAsync(id.Value);
        if (customer is null) return NotFound();
        Customer = customer;

        var contact = await _service.FindContactForUpdateAsync(contactId.Value);
        if (contact is null) return NotFound();
        UpdateContact = contact;

        foreach (var operation in CustomerOperation.AllOperations) await SetPermissionAsync(operation);

        if (UserCan[CustomerOperation.Edit] && UpdateContact is { IsDeleted: false, CustomerIsDeleted: false })
            return Page();

        if (!UserCan[CustomerOperation.ManageDeletions]) return NotFound();
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted customer or contact.");
        return RedirectToPage("Details", new { id });
    }

    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("../Index");

        foreach (var operation in CustomerOperation.AllOperations) await SetPermissionAsync(operation);
        if (!UserCan[CustomerOperation.Edit]) return Forbid();

        var customer = await _service.FindBasicInfoAsync(id.Value);
        if (customer is null) return BadRequest();

        Customer = customer;
        if (Customer.IsDeleted) return BadRequest();

        await _validator.ApplyValidationAsync(UpdateContact, ModelState);
        if (!ModelState.IsValid) return Page();

        await _service.UpdateContactAsync(UpdateContact);
        
        HighlightId = UpdateContact.Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Contact successfully updated.");
        return RedirectToPage("Details", new { id });
    }

    private async Task SetPermissionAsync(IAuthorizationRequirement operation) =>
        UserCan[operation] = (await _authorization.AuthorizeAsync(User, UpdateContact, operation)).Succeeded;
}
