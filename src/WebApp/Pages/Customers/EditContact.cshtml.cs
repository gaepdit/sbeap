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
public class EditContactModel(
    ICustomerService service,
    IValidator<ContactUpdateDto> validator,
    IValidator<PhoneNumberCreate> phoneValidator,
    IAuthorizationService authorization)
    : PageModel
{
    // Properties

    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public ContactUpdateDto ContactUpdate { get; set; } = default!;

    [BindProperty]
    public PhoneNumberCreate NewPhoneNumber { get; set; } = default!;

    [TempData]
    public Guid HighlightId { get; set; }

    public CustomerSearchResultDto CustomerView { get; private set; } = default!;
    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public static SelectList StatesSelectList => new(StateData.States);

    // Messaging
    public Handlers Handler { get; private set; } = Handlers.None;
    public DisplayMessage? PhoneNumberMessage { get; set; }

    public enum Handlers
    {
        None,
        EditContact,
        PhoneNumber,
    }

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        // id is Contact ID
        if (id is null) return RedirectToPage("Index");

        var contact = await service.FindContactForUpdateAsync(id.Value);
        if (contact is null) return NotFound();

        var customer = await service.FindBasicInfoAsync(contact.CustomerId);
        if (customer is null) return NotFound();

        await SetPermissionsAsync(contact);

        if (UserCan[CustomerOperation.Edit])
        {
            Id = id.Value;
            ContactUpdate = contact;
            CustomerView = customer;
            PhoneNumberMessage = TempData.Get<DisplayMessage>(nameof(PhoneNumberMessage));
            return Page();
        }

        if (!UserCan[CustomerOperation.ManageDeletions])
            return NotFound();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted customer or contact.");
        return RedirectToPage("Details", new { id = contact.CustomerId });
    }

    public async Task<IActionResult> OnPostSaveContactAsync()
    {
        var originalContact = await RefreshExistingData(Id);
        if (originalContact is null) return BadRequest();

        await SetPermissionsAsync(originalContact);
        if (!UserCan[CustomerOperation.Edit]) return BadRequest();

        Handler = Handlers.EditContact;

        await validator.ApplyValidationAsync(ContactUpdate, ModelState);
        if (!ModelState.IsValid) return Page();

        await service.UpdateContactAsync(Id, ContactUpdate);

        HighlightId = Id;
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Contact successfully updated.");
        return RedirectToPage("Details", new { id = ContactUpdate.CustomerId });
    }

    public async Task<IActionResult> OnPostAddPhoneNumberAsync()
    {
        var originalContact = await RefreshExistingData(Id);
        if (originalContact is null) return BadRequest();

        await SetPermissionsAsync(originalContact);
        if (!UserCan[CustomerOperation.Edit]) return BadRequest();

        Handler = Handlers.PhoneNumber;

        await phoneValidator.ApplyValidationAsync(NewPhoneNumber, ModelState);
        if (ContactUpdate.PhoneNumbers.Exists(p => p.Number == NewPhoneNumber.Number))
            ModelState.AddModelError(nameof(NewPhoneNumber.Number), "Phone number already exists.");

        if (!ModelState.IsValid) return Page();

        var result = await service.AddPhoneNumberAsync(Id, NewPhoneNumber);
        ContactUpdate.PhoneNumbers.Add(result);
        ModelState.Remove("NewPhoneNumber.Number");
        ModelState.Remove("NewPhoneNumber.Type");
        NewPhoneNumber = new PhoneNumberCreate();

        TempData.Set(nameof(PhoneNumberMessage),
            new DisplayMessage(DisplayMessage.AlertContext.Success, "New phone number added."));
        return RedirectToPage("EditContact", null, new { Id });
    }

    public async Task<IActionResult> OnPostDeletePhoneNumberAsync(int? phoneNumberId)
    {
        if (phoneNumberId is null) return BadRequest();

        var originalContact = await RefreshExistingData(Id);
        if (originalContact is null) return BadRequest();

        await SetPermissionsAsync(originalContact);
        if (!UserCan[CustomerOperation.Edit]) return BadRequest();

        Handler = Handlers.PhoneNumber;

        await service.DeletePhoneNumberAsync(Id, phoneNumberId.Value);
        ContactUpdate.PhoneNumbers.RemoveAll(p => p.Id == phoneNumberId);

        TempData.Set(nameof(PhoneNumberMessage),
            new DisplayMessage(DisplayMessage.AlertContext.Success, "Phone number deleted."));
        return RedirectToPage("EditContact", null, new { Id });
    }

    private async Task SetPermissionsAsync(ContactUpdateDto item)
    {
        foreach (var operation in CustomerOperation.AllOperations)
            UserCan[operation] = (await authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }

    private async Task<ContactUpdateDto?> RefreshExistingData(Guid id)
    {
        var originalContact = await service.FindContactForUpdateAsync(id);
        if (originalContact is null) return null;

        var customer = await service.FindBasicInfoAsync(originalContact.CustomerId);
        if (customer is null) return null;

        ContactUpdate.IsDeleted = originalContact.IsDeleted;
        ContactUpdate.CustomerIsDeleted = originalContact.CustomerIsDeleted;
        ContactUpdate.CustomerId = originalContact.CustomerId;
        ContactUpdate.PhoneNumbers.Clear();
        ContactUpdate.PhoneNumbers.AddRange(originalContact.PhoneNumbers);

        CustomerView = customer;

        return originalContact;
    }
}
