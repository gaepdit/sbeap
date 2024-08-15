﻿namespace Sbeap.WebApp.Models;

public record DisplayMessage(DisplayMessage.AlertContext Context, string Message)
{
    public string AlertClass => Context switch
    {
        AlertContext.Primary => "alert-primary",
        AlertContext.Secondary => "alert-secondary",
        AlertContext.Success => "alert-success",
        AlertContext.Danger => "alert-danger",
        AlertContext.Warning => "alert-warning",
        AlertContext.Info => "alert-info",
        _ => string.Empty,
    };

    public enum AlertContext
    {
        Primary,
        Secondary,
        Success,
        Danger,
        Warning,
        Info,
    }
}
