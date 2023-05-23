using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sbeap.AppServices.Cases;
using Sbeap.AppServices.Cases.Dto;
using Sbeap.AppServices.Cases.Permissions;
using Sbeap.AppServices.Permissions;
using Sbeap.WebApp.Models;
using Sbeap.WebApp.Platform.PageModelHelpers;
using System.ComponentModel.DataAnnotations;

namespace Sbeap.WebApp.Pages.Cases;

[Authorize(Policy = PolicyName.AdminUser)]
public class DeleteModel : PageModel
{
    // Constructor
    private readonly ICaseworkService _service;
    private readonly IAuthorizationService _authorization;

    public DeleteModel(
        ICaseworkService service,
        IAuthorizationService authorization)
    {
        _service = service;
        _authorization = authorization;
    }

    // Properties
    [BindProperty]
    [HiddenInput]
    public Guid Id { get; set; }

    [BindProperty]
    [Display(Name = "Deletion Comments (optional)")]
    public string? DeleteComments { get; set; }

    public CaseworkViewDto Item { get; private set; } = default!;

    // Methods
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if (id is null) return RedirectToPage("Index");
        var item = await _service.FindAsync(id.Value);
        if (item is null) return NotFound();

        Id = id.Value;
        Item = item;

        if (!await UserCanManageDeletions()) return Forbid();
        if (!Item.IsDeleted) return Page();

        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Case is already deleted.");
        return RedirectToPage("Details", new { Id });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var item = await _service.FindAsync(Id);
        if (item is null) return NotFound();

        Item = item;
        if (!await UserCanManageDeletions()) return Forbid();

        if (Item.IsDeleted)
        {
            TempData.SetDisplayMessage(DisplayMessage.AlertContext.Info, "Case is already deleted.");
            return RedirectToPage("Details", new { Id });
        }

        if (!ModelState.IsValid) return Page();

        await _service.DeleteAsync(Id, DeleteComments);
        TempData.SetDisplayMessage(DisplayMessage.AlertContext.Success, "Case successfully deleted.");
        return RedirectToPage("Details", new { Id });
    }

    private async Task<bool> UserCanManageDeletions() =>
        (await _authorization.AuthorizeAsync(User, Item, CaseworkOperation.ManageDeletions)).Succeeded;
}
