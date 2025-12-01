using Sbeap.AppServices.AutoMapper;
using Sbeap.AppServices.ServiceRegistration;
using Sbeap.WebApp.Platform.AppConfiguration;
using Sbeap.WebApp.Platform.OrgNotifications;
using Sbeap.WebApp.Platform.Settings;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders().AddZLoggerConsole(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UsePlainTextFormatter();
    else
        options.UseJsonFormatter();
});
// Set the default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices-regex#use-time-out-values
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(100));

// Configure basic settings.
builder.BindAppSettings().AddSecurityHeaders().AddErrorLogging();
builder.Services.AddDataProtection();

// Configure authentication and authorization.
builder.ConfigureAuthentication();

// Add UI services.
builder.Services.AddRazorPages();

// Add app services.
builder.Services.AddAppServices().AddAutoMapperProfiles().AddIdentityStores().AddWebOptimizer().AddMemoryCache()
    .AddOrgNotifications();

// Add data stores and initialize the database.
await builder.ConfigureDataPersistenceAsync();

// Build the application.
var app = builder.Build();

// Configure the application pipeline.
app.UseSecurityHeaders().UseErrorHandling().UseStatusCodePagesWithReExecute("/Error/{0}").UseHttpsRedirection()
    .UseWebOptimizer().UseStaticFiles().UseRouting().UseAuthentication().UseAuthorization();

// Map endpoints.
app.MapRazorPages();
app.MapControllers();

// Make it so.
await app.RunAsync();
