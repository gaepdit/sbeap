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
public class EditContactModel : PageModel
{
    // Constructor
    private readonly ICustomerService _service;
    private readonly IValidator<ContactUpdateDto> _validator;
    private readonly IValidator<PhoneNumberCreate> _phoneValidator;
    private readonly IAuthorizationService _authorization;

    public EditContactModel(
        ICustomerService service,
        IValidator<ContactUpdateDto> validator,
        IValidator<PhoneNumberCreate> phoneValidator,
        IAuthorizationService authorization)
    {
        _service = service;
        _validator = validator;
        _phoneValidator = phoneValidator;
        _authorization = authorization;
    }

    // Properties
    [BindProperty]
    public ContactUpdateDto ContactUpdate { get; set; } = default!;

    [BindProperty]
    public PhoneNumberCreate NewPhoneNumber { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CustomerSearchResultDto CustomerView { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public SelectList StatesSelectList => new(Data.States);

    // Messaging
    public Handlers Handler { get; private set; } = Handlers.None;
    public DisplayMessage? PhoneNumberMessage { get; private set; }

    public enum Handlers
    {
        None,
        EditContact,
        PhoneNumber,
    }

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");

        var contact = await _service.FindContactForUpdateAsync(id.Value);
        if (contact is null) return NotFound();
        ContactUpdate = contact;

        var customer = await _service.FindBasicInfoAsync(contact.CustomerId);
        if (customer is null) return NotFound();
        CustomerView = customer;

        foreach (var operation in CustomerOperation.AllOperations) await SetPermissionAsync(operation);

        if (UserCan[CustomerOperation.Edit]) return Page();
        if (!UserCan[CustomerOperation.ManageDeletions]) return NotFound();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted customer or contact.");
        return RedirectToPage("Details", new { id = ContactUpdate.CustomerId });
    }

    public async Task<IActionResult> OnPostSaveContactAsync(Guid? id)
    {
        if (id is null || id != ContactUpdate.Id) return RedirectToPage("Index");
        if (!await RefreshExistingData(id.Value)) return BadRequest();

        foreach (var operation in CustomerOperation.AllOperations) await SetPermissionAsync(operation);
        if (!UserCan[CustomerOperation.Edit]) return Forbid();

        Handler = Handlers.EditContact;

        await _validator.ApplyValidationAsync(ContactUpdate, ModelState);
        if (!ModelState.IsValid) return Page();

        await _service.UpdateContactAsync(ContactUpdate);

        HighlightId = ContactUpdate.Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Contact successfully updated.");
        return RedirectToPage("Details", null, new { id = ContactUpdate.CustomerId }, id.ToString());
    }

    public async Task<IActionResult> OnPostAddPhoneNumberAsync(Guid? id)
    {
        if (id is null || id != ContactUpdate.Id) return RedirectToPage("Index");
        if (!await RefreshExistingData(id.Value)) return BadRequest();

        Handler = Handlers.PhoneNumber;

        await _phoneValidator.ApplyValidationAsync(NewPhoneNumber, ModelState);
        if (ContactUpdate.PhoneNumbers.Exists(p => p.Number == NewPhoneNumber.Number))
            ModelState.AddModelError(nameof(NewPhoneNumber.Number), "Phone number already exists.");

        if (!ModelState.IsValid) return Page();

        var result = await _service.AddPhoneNumberAsync(id.Value, NewPhoneNumber);
        ContactUpdate.PhoneNumbers.Add(result);
        ModelState.Remove("NewPhoneNumber.Number");
        ModelState.Remove("NewPhoneNumber.Type");
        NewPhoneNumber = new PhoneNumberCreate();

        PhoneNumberMessage = new DisplayMessage(DisplayMessage.AlertContext.Success, "New phone number added.");
        return Page();
    }

    public async Task<IActionResult> OnPostDeletePhoneNumberAsync(Guid? id, int? phoneNumberId)
    {
        if (id is null || id != ContactUpdate.Id || phoneNumberId is null) return RedirectToPage("Index");
        if (!await RefreshExistingData(id.Value)) return BadRequest();

        Handler = Handlers.PhoneNumber;

        await _service.DeletePhoneNumberAsync(id.Value, phoneNumberId.Value);
        ContactUpdate.PhoneNumbers.RemoveAll(p => p.Id == phoneNumberId);

        PhoneNumberMessage = new DisplayMessage(DisplayMessage.AlertContext.Success, "Phone number deleted.");
        return Page();
    }

    private async Task SetPermissionAsync(IAuthorizationRequirement operation) =>
        UserCan[operation] = (await _authorization.AuthorizeAsync(User, ContactUpdate, operation)).Succeeded;

    private async Task<bool> RefreshExistingData(Guid id)
    {
        var originalContact = await _service.FindContactForUpdateAsync(id);
        if (originalContact is null) return false;
        ContactUpdate.IsDeleted = originalContact.IsDeleted;
        ContactUpdate.CustomerIsDeleted = originalContact.CustomerIsDeleted;
        ContactUpdate.CustomerId = originalContact.CustomerId;
        ContactUpdate.PhoneNumbers.Clear();
        ContactUpdate.PhoneNumbers.AddRange(originalContact.PhoneNumbers);

        var customer = await _service.FindBasicInfoAsync(originalContact.CustomerId);
        if (customer is null) return false;

        CustomerView = customer;
        return true;
    }
}
