using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;

namespace Sbeap.WebApp.Pages.Dev;

// TODO: Remove this page once testing of error handling is complete.
[AllowAnonymous]
public class ThrowError : PageModel
{
    [SuppressMessage("Major Code Smell", "S112:General exceptions should never be thrown")]
    public void OnGet()
    {
        throw new Exception("Test exception");
    }
}
