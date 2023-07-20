using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using Mindscape.Raygun4Net.AspNetCore;
using Sbeap.AppServices.RegisterServices;
using Sbeap.WebApp.Platform.Raygun;
using Sbeap.WebApp.Platform.Services;
using Sbeap.WebApp.Platform.Settings;

var builder = WebApplication.CreateBuilder(args);

// Set default timeout for regular expressions.
// https://learn.microsoft.com/en-us/dotnet/standard/base-types/best-practices#use-time-out-values
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
if (!string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey))
{
    builder.Services.AddTransient<IErrorLogger, ErrorLogger>();
    builder.Services.AddRaygun(builder.Configuration,
        new RaygunMiddlewareSettings { ClientProvider = new RaygunClientProvider() });
    builder.Services.AddHttpContextAccessor(); // needed by RaygunScriptPartial
}

// Add app services.
builder.Services.AddAutoMapperProfiles();
builder.Services.AddAppServices();
builder.Services.AddValidators();

// Add data stores.
builder.Services.AddDataStores(builder.Configuration);

// Initialize database.
builder.Services.AddHostedService<MigratorHostedService>();

// Add API documentation.
builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SBEAP API",
    });
    opts.CustomSchemaIds(e => e.FullName);
});

// Configure bundling and minification.
builder.Services.AddWebOptimizer();

// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Development
    app.UseDeveloperExceptionPage();
}
else
{
    // Production or Staging
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    if (!string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey)) app.UseRaygun();
}

// Configure the application pipeline.
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHttpsRedirection();
app.UseWebOptimizer();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configure API documentation.
app.UseSwagger(opts => { opts.RouteTemplate = "api-docs/{documentName}/openapi.json"; });
app.UseSwaggerUI(opts =>
{
    opts.SwaggerEndpoint("v1/openapi.json", "SBEAP API v1");
    opts.RoutePrefix = "api-docs";
    opts.DocumentTitle = "SBEAP API";
});

// Map endpoints.
app.MapRazorPages();
app.MapControllers();

// Make it so.
app.Run();
