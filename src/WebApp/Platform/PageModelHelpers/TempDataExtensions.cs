using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Sbeap.WebApp.Models;
using System.Text.Json;

namespace Sbeap.WebApp.Platform.PageModelHelpers;

public static class TempDataExtensions
{
    extension(ITempDataDictionary tempData)
    {
        internal void Set<T>(string key, T value) where T : class => tempData[key] = JsonSerializer.Serialize(value);

        internal T? Get<T>(string key) where T : class
        {
            tempData.TryGetValue(key, out var o);
            return o is null ? null : JsonSerializer.Deserialize<T>((string)o);
        }

        public void SetDisplayMessage(DisplayMessage.AlertContext context, string message) =>
            tempData.Set(nameof(DisplayMessage), new DisplayMessage(context, message));

        public DisplayMessage? GetDisplayMessage() => tempData.Get<DisplayMessage>(nameof(DisplayMessage));
    }
}
