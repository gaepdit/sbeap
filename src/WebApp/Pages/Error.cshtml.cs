using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZLogger;

namespace Sbeap.WebApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
#pragma warning disable S4502 // Make sure disabling CSRF protection is safe here. 
[IgnoreAntiforgeryToken]
#pragma warning restore S4502
[AllowAnonymous]
public class ErrorModel(ILogger<ErrorModel> logger) : PageModel
{
    public int? Status { get; private set; }

    public void OnGet(int? statusCode)
    {
        if (statusCode is null)
        {
            logger.ZLogError($"Error page shown from Get method");
        }
        else
        {
            logger.ZLogError($"Error page shown from Get method with status code {statusCode}");
        }

        Status = statusCode;
    }

    public void OnPost() => logger.ZLogError($"Error page shown from Post method");
}
