using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.AspNetCore;
using Sbeap.AppServices.RegisterServices;
using Sbeap.WebApp.Platform.Raygun;
using Sbeap.WebApp.Platform.SecurityHeaders;
using Sbeap.WebApp.Platform.Services;
using Sbeap.WebApp.Platform.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
// ReSharper disable once HeapView.BoxingAllocation
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Bind application settings.
AppConfiguration.BindSettings(builder);

// Configure Identity.
builder.Services.AddIdentityStores();

// Configure Authentication.
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformation>();

// Persist data protection keys.
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"] ?? "", "DataProtectionKeys");
builder.Services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));

// Configure authorization policies.
builder.Services.AddAuthorizationPolicies();

// Configure UI services.
builder.Services.AddRazorPages();

// Starting value for HSTS max age is five minutes to allow for debugging.
// For more info on updating HSTS max age value for production, see:
// https://gaepdit.github.io/web-apps/use-https.html#how-to-enable-hsts
if (!builder.Environment.IsDevelopment())
    builder.Services.AddHsts(opts => opts.MaxAge = TimeSpan.FromMinutes(300));

// Configure application monitoring.
builder.Services.AddTransient<IErrorLogger, ErrorLogger>();
if (!string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey))
{
    builder.Services.AddSingleton(provider =>
    {
        var client = new RaygunClient(provider.GetService<RaygunSettings>()!,
            provider.GetService<IRaygunUserProvider>()!);
        client.SendingMessage += (_, eventArgs) =>
            eventArgs.Message.Details.Tags.Add(builder.Environment.EnvironmentName);
        return client;
    });
    builder.Services.AddRaygun(opts =>
    {
        opts.ApiKey = ApplicationSettings.RaygunSettings.ApiKey;
        opts.ApplicationVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3);
        opts.ExcludeErrorsFromLocal = ApplicationSettings.RaygunSettings.ExcludeErrorsFromLocal;
        opts.IgnoreFormFieldNames = ["*Password"];
        opts.EnvironmentVariables.Add("ASPNETCORE_*");
    });
    builder.Services.AddRaygunUserProvider();
}

// Add app services.
builder.Services.AddAutoMapperProfiles();
builder.Services.AddAppServices();
builder.Services.AddValidators();

// Add data stores.
builder.Services.AddDataStores(builder.Configuration);

// Initialize database.
builder.Services.AddHostedService<MigratorHostedService>();

// Configure bundling and minification.
builder.Services.AddWebOptimizer();

//Add simple cache.
builder.Services.AddMemoryCache();

// Build the application.
var app = builder.Build();

// Configure error handling.
// if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage(); // Development
// else 
app.UseExceptionHandler("/Error"); // Production or Staging

// Configure security HTTP headers
if (!app.Environment.IsDevelopment() || ApplicationSettings.DevSettings.UseSecurityHeadersInDev)
{
    app.UseHsts();
    app.UseSecurityHeaders(policyCollection => policyCollection.AddSecurityHeaderPolicies());
}

if (!string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey)) app.UseRaygun();

// Configure the application pipeline.
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHttpsRedirection();
app.UseWebOptimizer();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints.
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();
