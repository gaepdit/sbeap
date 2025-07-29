using Microsoft.AspNetCore.Authentication;
using Sbeap.AppServices.RegisterServices;
using Sbeap.WebApp.Platform.AppConfiguration;
using Sbeap.WebApp.Platform.OrgNotifications;
using Sbeap.WebApp.Platform.Services;
using Sbeap.WebApp.Platform.Settings;

var builder = WebApplication.CreateBuilder(args);

// Set the default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-regex#use-time-out-values
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Persist data protection keys.
builder.Services.AddDataProtection();

// Bind application settings.
builder.BindAppSettings().AddSecurityHeaders().AddErrorLogging();

// Configure Identity.
builder.Services.AddIdentityStores();

// Configure Authentication.
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformation>();

// Configure authorization policies.
builder.Services.AddAuthorizationPolicies();

// Configure UI services.
builder.Services.AddRazorPages();

// Add app services.
builder.Services.AddAutoMapperProfiles();
builder.Services.AddAppServices();
builder.Services.AddValidators();

// Add data stores and initialize the database.
await builder.ConfigureDataPersistence();

// Configure bundling and minification.
builder.Services.AddWebOptimizer();

//Add simple cache.
builder.Services.AddMemoryCache();

// Add organizational notifications.
builder.Services.AddOrgNotifications();

// Build the application.
var app = builder.Build();

// Configure the application pipeline.
app
    .UseSecurityHeaders()
    .UseErrorHandling()
    .UseStatusCodePagesWithReExecute("/Error/{0}")
    .UseHttpsRedirection()
    .UseWebOptimizer()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

// Map endpoints.
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();
