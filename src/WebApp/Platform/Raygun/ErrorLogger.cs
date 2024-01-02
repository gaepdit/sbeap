﻿using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net.AspNetCore;

namespace Sbeap.WebApp.Platform.Raygun;

public class ErrorLogger(
    IRaygunAspNetCoreClientProvider clientProvider,
    IOptions<RaygunSettings> settings,
    IHttpContextAccessor httpContextAccessor)
    : IErrorLogger
{
    public Task LogErrorAsync(Exception exception, Dictionary<string, object>? customData = null) =>
        clientProvider.GetClient(settings.Value, httpContextAccessor.HttpContext)
            .SendInBackground(exception, tags: null, customData);
}

public interface IErrorLogger
{
    /// <summary>
    /// Asynchronously transmits an exception to the error logger.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should only be used to manually send handled exceptions from within a try/catch block. (Unhandled
    /// exceptions are automatically sent.)
    /// </para>
    /// <para>
    /// See Raygun's documentation for more info: 
    /// https://raygun.com/documentation/language-guides/dotnet/crash-reporting/aspnetcore/#manually-sending-exceptions
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    ///   try
    ///   {
    ///       // Code with potential exception.
    ///   }
    ///   catch (Exception e)
    ///   {
    ///       await _errorLogger.LogErrorAsync(e);
    ///   }
    /// </code>
    /// </example>
    /// <param name="exception">The exception to deliver.</param>
    /// <param name="customData">An optional key-value collection of custom data that will be added to the payload.</param>
    // ReSharper disable once UnusedMember.Global
    Task LogErrorAsync(Exception exception, Dictionary<string, object>? customData = null);
}
