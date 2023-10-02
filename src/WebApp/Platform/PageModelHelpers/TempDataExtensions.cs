using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Sbeap.WebApp.Models;
using System.Text.Json;

namespace Sbeap.WebApp.Platform.PageModelHelpers;

public static class TempDataExtensions
{
    internal static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
    {
        tempData[key] = JsonSerializer.Serialize(value);
    }

    internal static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        tempData.TryGetValue(key, out var o);
        return o is null ? null : JsonSerializer.Deserialize<T>((string)o);
    }

    public static void SetDisplayMessage(this ITempDataDictionary tempData, DisplayMessage.AlertContext context,
        string message)
    {
        tempData.Set(nameof(DisplayMessage), new DisplayMessage(context, message));
    }

    public static DisplayMessage? GetDisplayMessage(this ITempDataDictionary tempData) =>
        tempData.Get<DisplayMessage>(nameof(DisplayMessage));
}
