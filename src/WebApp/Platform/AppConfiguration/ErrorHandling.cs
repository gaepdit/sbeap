﻿using Mindscape.Raygun4Net.AspNetCore;
using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.AppConfiguration;

internal static class ErrorHandling
{
    public static void AddErrorLogging(this WebApplicationBuilder builder)
    {
        if (string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) return;

        builder.Services.AddRaygun(options =>
        {
            options.ApiKey = AppSettings.RaygunSettings.ApiKey;
            options.ApplicationVersion = AppSettings.Version;
            options.IgnoreFormFieldNames = ["*Password"];
            options.EnvironmentVariables.Add("ASPNETCORE_*");
        });
        builder.Services.AddRaygunUserProvider();
    }

    public static WebApplication UseErrorHandling(this WebApplication app)
    {
        if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage(); // Development
        else app.UseExceptionHandler("/Error"); // Production or Staging

        if (!string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) app.UseRaygun();

        return app;
    }
}
