﻿using FluentValidation;
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

    [FromRoute]
    public Guid Id { get; set; }

    [BindProperty]
    public CustomerUpdateDto Item { get; set; } = default!;

    public Dictionary<IAuthorizationRequirement, bool> UserCan { get; set; } = new();

    // Select lists
    public SelectList StatesSelectList => new(Data.States);
    public SelectList CountiesSelectList => new(Data.Counties);

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await _service.FindForUpdateAsync(id.Value);
        if (item is null) return NotFound();

        await SetPermissionsAsync(item);

        if (UserCan[CustomerOperation.Edit])
        {
            Id = id.Value;
            Item = item;
            return Page();
        }

        if (!UserCan[CustomerOperation.ManageDeletions]) return NotFound();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Cannot edit a deleted customer.");
        return RedirectToPage("Details", new { id });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var originalItem = await _service.FindForUpdateAsync(Id);
        if (originalItem is null) return BadRequest();
        await SetPermissionsAsync(originalItem);
        if (!UserCan[CustomerOperation.Edit]) return BadRequest();

        await _validator.ApplyValidationAsync(Item, ModelState);
        if (!ModelState.IsValid) return Page();

        await _service.UpdateAsync(Id, Item);

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Customer successfully updated.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task SetPermissionsAsync(CustomerUpdateDto item)
    {
        foreach (var operation in CustomerOperation.AllOperations)
            UserCan[operation] = (await _authorization.AuthorizeAsync(User, item, operation)).Succeeded;
    }
}
