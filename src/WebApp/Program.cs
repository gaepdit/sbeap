using Microsoft.AspNetCore.DataProtection;
using Mindscape.Raygun4Net.AspNetCore;
using Sbeap.AppServices.ServiceCollectionExtensions;
using Sbeap.WebApp.Platform.Local;
using Sbeap.WebApp.Platform.Raygun;
using Sbeap.WebApp.Platform.Services;
using Sbeap.WebApp.Platform.Settings;

var builder = WebApplication.CreateBuilder(args);
var isLocal = builder.Environment.IsLocalEnv();

// Bind application settings.
builder.Configuration.GetSection(nameof(ApplicationSettings.LocalDevSettings))
    .Bind(ApplicationSettings.LocalDevSettings);
builder.Configuration.GetSection(nameof(ApplicationSettings.RaygunSettings))
    .Bind(ApplicationSettings.RaygunSettings);
var raygunApiKeySet = !string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey);

// Configure Identity.
builder.Services.AddIdentityStores(isLocal);

// Configure cookies (SameSiteMode.None is needed to get single sign-out to work).
builder.Services.Configure<CookiePolicyOptions>(opts => opts.MinimumSameSitePolicy = SameSiteMode.None);

// Configure Authentication.
builder.Services.AddAuthenticationServices(builder.Configuration, isLocal);

// Persist data protection keys.
var keysFolder = Path.Combine(builder.Configuration["PersistedFilesBasePath"], "DataProtectionKeys");
builder.Services.AddDataProtection().PersistKeysToFileSystem(Directory.CreateDirectory(keysFolder));
builder.Services.AddAuthorization();

// Configure UI services.
builder.Services.AddRazorPages();

// Starting value for HSTS max age is five minutes to allow for debugging.
// For more info on updating HSTS max age value for production, see:
// https://gaepdit.github.io/web-apps/use-https.html#how-to-enable-hsts
if (!isLocal) builder.Services.AddHsts(opts => opts.MaxAge = TimeSpan.FromMinutes(300));

// Configure application monitoring.
if (raygunApiKeySet)
{
    builder.Services.AddTransient<IErrorLogger, ErrorLogger>();
    builder.Services.AddRaygun(builder.Configuration,
        new RaygunMiddlewareSettings { ClientProvider = new RaygunClientProvider() });
    builder.Services.AddHttpContextAccessor(); // needed by RaygunScriptPartial
}

// Add App and data services.
builder.Services.AddAppServices();
builder.Services.AddDataServices(builder.Configuration, isLocal);

// Initialize database.
builder.Services.AddHostedService<MigratorHostedService>();

// Configure bundling and minification.
builder.Services.AddWebOptimizer();

// Build the application.
var app = builder.Build();
var env = app.Environment;

// Configure the HTTP request pipeline.
if (env.IsProduction() || env.IsStaging())
{
    // Production or Staging
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    if (raygunApiKeySet) app.UseRaygun();
}
else
{
    // Development or Local
    app.UseDeveloperExceptionPage();
}

// Configure the application pipeline.
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseWebOptimizer();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints.
app.MapRazorPages();

// Make it so.
app.Run();
