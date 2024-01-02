namespace Sbeap.WebApp.Models;

public class DisplayMessage(DisplayMessage.AlertContext context, string message)
{
    // Context must be public so it works with deserialization in TempDataExtensions class
    public AlertContext Context => context;

    public string Message => message;

    public string AlertClass => Context switch
    {
        AlertContext.Primary => "alert-primary",
        AlertContext.Secondary => "alert-secondary",
        AlertContext.Success => "alert-success",
        AlertContext.Danger => "alert-danger",
        AlertContext.Info => "alert-info",
        _ => string.Empty,
    };

    public enum AlertContext
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Info,
    }
}
